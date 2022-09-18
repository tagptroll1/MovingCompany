using Dapper;
using Dapper.Contrib.Extensions;
using MovingCompany.Framework;
using MovingCompany.Models;
using MovingCompany.Modules.Interfaces;
using System.Text;

namespace MovingCompany.Modules.DataAccess;

public class MovingOrdersService: IMovingOrderService
{
    private readonly string _connectionString;

    public MovingOrdersService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Database");
    }

    #region CRUD
    public async Task<IEnumerable<MovingOrder>> GetMovingOrdersAsync(int? customerID = null, params int[] IDs)
    {

        var queryBuilder = new StringBuilder("SELECT * FROM MovingOrder o WHERE o.Deleted = false");

        if (IDs is not null && IDs.Length > 0)
        {
            queryBuilder.Append(" AND o.ID = ANY(@IDs)");
        }

        if (customerID is not null)
        {
            queryBuilder.Append(" AND o.CustomerID = @customerID");
        }
        using var connection = SqlConnectionFactory.GetConnection(_connectionString);
        return await connection.QueryAsync<MovingOrder>(queryBuilder.ToString(), new { IDs, customerID }
        );
    }

    public async Task<MovingOrder> CreateMovingOrderAsync(MovingOrder order)
    {
        ValidateOrderOrThrow(order);

        using var connection = SqlConnectionFactory.GetConnection(_connectionString);

        order.MoveFrom = null;
        order.MoveTo = null;

        var identity = await connection.InsertAsync(order);

        order.ID = identity;

        return order;
    }

    public async Task UpdateMovingOrder(int id, MovingOrder order)
    {
        order.ID = id;

        using var connection = SqlConnectionFactory.GetConnection(_connectionString);
        //var oldOrder = await connection.QueryFirstOrDefaultAsync<MovingOrder>(
        //    "SELECT * MovingOrder Where ID = @id", new { id });

        //if (oldOrder is null)
        //{
        //    throw new ArgumentException($"Movingorder with id {} does not exist");
        //}

        await connection.UpdateAsync(order);
    }

    public async Task DeleteMovingOrderAsync(int orderID)
    {
        using var connection = SqlConnectionFactory.GetConnection(_connectionString);
        await connection.UpdateAsync(new MovingOrder { ID = orderID, Deleted = true });
    }
    #endregion

    public async Task<IEnumerable<MovingOrder>> GetMovingOrdersExpandedAsync(int? customerID = null, params int[] IDs)
    {
        using var connection = SqlConnectionFactory.GetConnection(_connectionString);

        var query = @"
            SELECT * FROM MovingOrder o
            LEFT JOIN Address movefrom ON movefrom.ID = o.MoveFromAddressID
            LEFT JOIN Address moveto ON moveto.ID = o.MoveToAddressID
            LEFT JOIN Customer c on c.ID = o.CustomerID
            WHERE o.Deleted = false";

        if (IDs is not null && IDs.Length > 0)
        {
            query += " AND o.ID = ANY(@IDs)";
        }

        if (customerID is not null)
        {
            query += " AND o.CustomerID = @customerID";
        }

        var result = await connection.QueryAsync<MovingOrder, Address, Address, Customer, MovingOrder>(
            query,
            (order, moveFrom, moveTo, customer) => 
            {
                order.MoveFrom = moveFrom;
                order.MoveTo = moveTo;
                order.Customer = customer;
                return order;
            },
            new { IDs, customerID }
        );
        return result;
    }

    public Task<MovingOrder?> GetByIdAsync(int id, bool expanded = false)
    {
        var task = expanded
            ? GetMovingOrdersExpandedAsync(customerID: null, id)
            : GetMovingOrdersAsync(customerID: null, id);
        
        return task.ContinueWith(results =>
        {
            return results.Result.FirstOrDefault();
        });
    }



    public Task<IEnumerable<MovingOrder>> GetMovingOrdersForCustomer(int customerID)
    {
        return GetMovingOrdersExpandedAsync(customerID: customerID);
    }

    public async Task<MovingOrder> CreateExpandedMovingOrderAsync(MovingOrder order)
    {
        ValidateOrderOrThrow(order);

        using var connection = SqlConnectionFactory.GetConnection(_connectionString);
        await connection.OpenAsync();
        var transaction = await connection.BeginTransactionAsync();

        if (order.MoveFrom is not null)
        {
            order.MoveFromAddressID = order.MoveFrom.ID 
                ?? await connection.InsertAsync(order.MoveFrom, transaction: transaction);

            order.MoveFrom.ID = order.MoveFromAddressID;
        }

        if (order.MoveTo is not null)
        {
            order.MoveToAddressID = order.MoveTo.ID 
                ?? await connection.InsertAsync(order.MoveTo, transaction: transaction);
        }

        var identity = await connection.InsertAsync(order, transaction: transaction);

        await transaction.CommitAsync();
        
        order.ID = identity;
        order.MoveTo = null;
        order.MoveFrom = null;

        return order;
    }

    private static void ValidateOrderOrThrow(MovingOrder order)
    {
        var addressIdNull = order?.MoveFromAddressID is null;
        var objectNull = order?.MoveFrom is null;

        if (addressIdNull && objectNull)
        {
            throw new ArgumentException("MoveFrom Address is missing");
        }

        addressIdNull = order?.MoveToAddressID is null;
        objectNull = order?.MoveTo is null;
        var serviceHasMoving = (order?.Service & MovingServiceEnum.Moving) == MovingServiceEnum.Moving;
        if (addressIdNull && objectNull && serviceHasMoving)
        {
            throw new ArgumentException($"MoveTo Address is missing");
        }
    }
}

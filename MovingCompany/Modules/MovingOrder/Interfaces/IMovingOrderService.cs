using MovingCompany.Models;

namespace MovingCompany.Modules.Interfaces;

public interface IMovingOrderService
{
    Task<MovingOrder> CreateExpandedMovingOrderAsync(MovingOrder order);
    Task<IEnumerable<MovingOrder>> GetMovingOrdersForCustomer(int customerID);
    Task<IEnumerable<MovingOrder>> GetMovingOrdersExpandedAsync(int? customerID = null, params int[] IDs);
    Task<MovingOrder?> GetByIdAsync(int id, bool expanded = false);
    Task<IEnumerable<MovingOrder>> GetMovingOrdersAsync(int? customerID = null, params int[] IDs);
    Task<MovingOrder> CreateMovingOrderAsync(MovingOrder order);
    Task UpdateMovingOrder(int id, MovingOrder order);
    Task DeleteMovingOrderAsync(int orderID);
}

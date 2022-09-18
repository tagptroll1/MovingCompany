CREATE TABLE IF NOT EXISTS MovingOrder (
    ID serial PRIMARY KEY,
    Service int,
    CustomerID int REFERENCES Customer(ID),
    MoveFromAddressID int REFERENCES Address(ID),
    MoveFromDate TIMESTAMP,
    MoveToAddressID int REFERENCES Address(ID),
    MoveToDate TIMESTAMP,
    PackingDate TIMESTAMP,
    CleaningDate TIMESTAMP,
    Comment VARCHAR(500),
    StatusCode int,
    Deleted boolean NOT NULL DEFAULT false
)
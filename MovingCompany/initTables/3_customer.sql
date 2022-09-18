CREATE TABLE IF NOT EXISTS Customer(
    ID serial PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    PhoneID int REFERENCES Phone(ID),
    Email VARCHAR(255),
    Deleted boolean NOT NULL DEFAULT false
) 
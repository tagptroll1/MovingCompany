CREATE TABLE IF NOT EXISTS Address(
    ID serial PRIMARY KEY,
    AddressLine1 VARCHAR(255),
    AddressLine2 VARCHAR(255),
    AddressLine3 VARCHAR(255),
    ZipCode VARCHAR(255),
    City VARCHAR(255),
    Country VARCHAR(255),
    CountryCode VARCHAR(10),
    Region VARCHAR(255),
    Deleted boolean NOT NULL DEFAULT false
)
CREATE TABLE IF NOT EXISTS Phone (
    ID serial PRIMARY KEY,
    PhoneNumber VARCHAR(50) NOT NULL,
    CountryCode VARCHAR(10) NOT NULL,
    Deleted boolean NOT NULL DEFAULT false
)

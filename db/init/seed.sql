CREATE TABLE IF NOT EXISTS Tags (
  id serial PRIMARY KEY,
  name text NOT NULL,
);

INSERT INTO Tags (TagId, TagName)
VALUES ('Food'),
       ('Drinks')
ON CONFLICT DO NOTHING;

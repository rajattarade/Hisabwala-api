CREATE TABLE IF NOT EXISTS Tags (
  TagId serial PRIMARY KEY,
  TagName text NOT NULL
);

INSERT INTO Tags (TagId, TagName)
VALUES (1, 'Food'),
       (2, 'Drinks')
ON CONFLICT DO NOTHING;

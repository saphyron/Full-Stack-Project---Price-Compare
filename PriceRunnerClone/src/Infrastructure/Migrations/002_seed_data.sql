USE price_runner;

-- Roller
INSERT INTO user_roles (user_role_name) VALUES
  ('Admin'),
  ('Analyst'),
  ('Viewer');

-- Brugere
-- admin / admin123!
-- analyst / analyst123!
-- viewer / viewer123!
-- user / user123!
INSERT INTO users (user_name, password_hash, user_role_id) VALUES
  ('admin',
   '5C06EB3D5A05A19F49476D694CA81A36344660E9D5B98E3D6A6630F31C2422E7',
   1),
  ('analyst',
   '0C43CF99262ACBCC228C680787A8AA76CCF19DC72C28FDA4EBF24D12AC959330',
   2),
  ('viewer',
   '4F1A37EE26ED45DFDB43250B09E0F68C915D34518DF25087E99C650AF49C5546',
   3),
  ('user',
   '056E84183058806CA077B74BBF0AEEB544C30C8A0DD800295A87B7054A1AFC6A',
   3);

-- Brands (id: 1..4)
INSERT INTO brands (brand_name) VALUES
  ('Apple'),
  ('Samsung'),
  ('Sony'),
  ('LG');

-- Categories (id: 1..4)
INSERT INTO categories (category_name) VALUES
  ('Smartphones'),
  ('Laptops'),
  ('TVs'),
  ('Headphones');

-- Shops (id: 1..3)
INSERT INTO shops (full_name, shop_url) VALUES
  ('TechWorld.dk',      'https://www.techworld.dk'),
  ('ElektronikLand',    'https://www.elektronikland.dk'),
  ('BudgetElectro.dk',  'https://www.budgetelectro.dk');

-- Products
-- (product_name, url, shop_id, brand_id, category_id)
INSERT INTO products (product_name, product_url, shop_id, brand_id, category_id) VALUES
  ('Apple iPhone 15 128GB',
   'https://example.com/apple-iphone-15-128',
   1,  -- TechWorld.dk
   1,  -- Apple
   1), -- Smartphones

  ('Samsung Galaxy S24 256GB',
   'https://example.com/samsung-galaxy-s24-256',
   2,  -- ElektronikLand
   2,  -- Samsung
   1), -- Smartphones

  ('Sony WH-1000XM5',
   'https://example.com/sony-wh-1000xm5',
   1,  -- TechWorld.dk
   3,  -- Sony
   4), -- Headphones

  ('LG C3 55\" OLED TV',
   'https://example.com/lg-c3-55-oled',
   3,  -- BudgetElectro.dk
   4,  -- LG
   3), -- TVs

  ('Apple MacBook Air 13\"',
   'https://example.com/apple-macbook-air-13',
   2,  -- ElektronikLand
   1,  -- Apple
   2); -- Laptops

INSERT INTO product_prices (current_price, last_updated, product_id, shop_id) VALUES
  (8999.00, '2025-11-20 10:00:00', 1, 1),
  (8799.00, '2025-11-21 10:00:00', 1, 2),

  (7999.00, '2025-11-20 11:00:00', 2, 2),

  (2499.00, '2025-11-20 12:00:00', 3, 1),

  (9999.00, '2025-11-20 13:00:00', 4, 3),

  (10499.00, '2025-11-21 09:30:00', 5, 2);

INSERT INTO products_history (price, recorded_at, product_id, shop_id) VALUES
  -- iPhone 15 hos TechWorld.dk (product 1, shop 1)
  (9499.00, '2025-11-01 10:00:00', 1, 1),
  (9299.00, '2025-11-05 10:00:00', 1, 1),
  (8999.00, '2025-11-10 10:00:00', 1, 1),

  -- Galaxy S24 hos ElektronikLand (product 2, shop 2)
  (8299.00, '2025-11-02 11:00:00', 2, 2),
  (8099.00, '2025-11-08 11:00:00', 2, 2),
  (7999.00, '2025-11-15 11:00:00', 2, 2),

  -- Sony WH-1000XM5 (product 3, shop 1)
  (2699.00, '2025-11-01 12:00:00', 3, 1),
  (2599.00, '2025-11-07 12:00:00', 3, 1),
  (2499.00, '2025-11-14 12:00:00', 3, 1),

  -- LG OLED TV (product 4, shop 3)
  (10999.00, '2025-11-03 13:00:00', 4, 3),
  (10499.00, '2025-11-10 13:00:00', 4, 3),
  (9999.00,  '2025-11-17 13:00:00', 4, 3);

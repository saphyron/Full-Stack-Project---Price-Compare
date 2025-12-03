use price_runner;

SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS user_roles;
CREATE TABLE user_roles (
  user_role_id int NOT NULL AUTO_INCREMENT,
  user_role_name varchar(255),
  PRIMARY KEY (user_role_id)
);

DROP TABLE IF EXISTS users;
CREATE TABLE users (
  user_id int NOT NULL AUTO_INCREMENT,
  user_name varchar(255) NOT NULL,
  password_hash varchar(255),
  user_role_id int,
  PRIMARY KEY (user_id),
  KEY user_role_id (user_role_id),
  CONSTRAINT user_role_id_cons FOREIGN KEY (user_role_id) REFERENCES user_roles (user_role_id)
);

DROP TABLE IF EXISTS brands;
CREATE TABLE brands (
  brand_id int NOT NULL AUTO_INCREMENT,
  brand_name varchar(255) NOT NULL,
  PRIMARY KEY (brand_id)
);

DROP TABLE IF EXISTS categories;
CREATE TABLE categories (
  category_id int NOT NULL AUTO_INCREMENT,
  category_name varchar(255) NOT NULL,
  PRIMARY KEY (category_id)
);

DROP TABLE IF EXISTS shops;
CREATE TABLE shops (
  shop_id int NOT NULL AUTO_INCREMENT,
  full_name varchar(255) NOT NULL,
  shop_url text,
  PRIMARY KEY (shop_id)
);

DROP TABLE IF EXISTS products;
CREATE TABLE products (
  product_id int NOT NULL AUTO_INCREMENT,
  product_name varchar(255) NOT NULL,
  product_url text,
  shop_id int,
  brand_id int NOT NULL,
  category_id int NOT NULL,
  PRIMARY KEY (product_id),
  KEY shop_id (shop_id),
  KEY brand_id (brand_id),
  KEY category_id (category_id),
  CONSTRAINT shop_conp FOREIGN KEY (shop_id) REFERENCES shops (shop_id),
  CONSTRAINT brand_cons FOREIGN KEY (brand_id) REFERENCES brands (brand_id),
  CONSTRAINT category_cons FOREIGN KEY (category_id) REFERENCES categories (category_id)
);

DROP TABLE IF EXISTS product_prices;
CREATE TABLE product_prices (
  product_price_id int NOT NULL AUTO_INCREMENT,
  current_price float DEFAULT NULL,
  last_updated datetime,
  product_id int,
  shop_id int,
  PRIMARY KEY (product_price_id),
  KEY product_id (product_id),
  KEY shop_id (shop_id),
  CONSTRAINT product_conpp FOREIGN KEY (product_id) REFERENCES products (product_id),
  CONSTRAINT shop_conpp FOREIGN KEY (shop_id) REFERENCES shops (shop_id)
);

DROP TABLE IF EXISTS product_histories;
CREATE TABLE products_history (
  products_history_id int NOT NULL AUTO_INCREMENT,
  price float DEFAULT NULL,
  recorded_at datetime,
  product_id int,
  shop_id int,
  PRIMARY KEY (products_history_id),
  KEY product_id (product_id),
  KEY shop_id (shop_id),
  CONSTRAINT product_conph FOREIGN KEY (product_id) REFERENCES products (product_id),
  CONSTRAINT shop_conph FOREIGN KEY (shop_id) REFERENCES shops (shop_id)
  );

SET FOREIGN_KEY_CHECKS = 1;
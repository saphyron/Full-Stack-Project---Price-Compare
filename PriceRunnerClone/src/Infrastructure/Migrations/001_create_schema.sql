-- 01_create_schema.sql
CREATE DATABASE IF NOT EXISTS price_runner
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE price_runner;

SET FOREIGN_KEY_CHECKS = 0;

-- Drop i "safe" rækkefølge
DROP TABLE IF EXISTS products_history;
DROP TABLE IF EXISTS product_prices;
DROP TABLE IF EXISTS products;
DROP TABLE IF EXISTS shops;
DROP TABLE IF EXISTS categories;
DROP TABLE IF EXISTS brands;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS user_roles;

CREATE TABLE user_roles (
  user_role_id   INT NOT NULL AUTO_INCREMENT,
  user_role_name VARCHAR(255),
  PRIMARY KEY (user_role_id)
);

CREATE TABLE users (
  user_id       INT NOT NULL AUTO_INCREMENT,
  user_name     VARCHAR(255) NOT NULL,
  password_hash VARCHAR(255),
  user_role_id  INT,
  PRIMARY KEY (user_id),
  KEY user_role_id (user_role_id),
  CONSTRAINT user_role_id_cons
    FOREIGN KEY (user_role_id)
    REFERENCES user_roles (user_role_id)
);

CREATE TABLE brands (
  brand_id   INT NOT NULL AUTO_INCREMENT,
  brand_name VARCHAR(255) NOT NULL,
  PRIMARY KEY (brand_id)
);

CREATE TABLE categories (
  category_id   INT NOT NULL AUTO_INCREMENT,
  category_name VARCHAR(255) NOT NULL,
  PRIMARY KEY (category_id)
);

CREATE TABLE shops (
  shop_id   INT NOT NULL AUTO_INCREMENT,
  full_name VARCHAR(255) NOT NULL,
  shop_url  TEXT,
  PRIMARY KEY (shop_id)
);

CREATE TABLE products (
  product_id   INT NOT NULL AUTO_INCREMENT,
  product_name VARCHAR(255) NOT NULL,
  product_url  TEXT,
  shop_id      INT,
  brand_id     INT NOT NULL,
  category_id  INT NOT NULL,
  PRIMARY KEY (product_id),
  KEY shop_id (shop_id),
  KEY brand_id (brand_id),
  KEY category_id (category_id),
  CONSTRAINT shop_conp
    FOREIGN KEY (shop_id) REFERENCES shops (shop_id),
  CONSTRAINT brand_cons
    FOREIGN KEY (brand_id) REFERENCES brands (brand_id),
  CONSTRAINT category_cons
    FOREIGN KEY (category_id) REFERENCES categories (category_id)
);

CREATE TABLE product_prices (
  product_price_id INT NOT NULL AUTO_INCREMENT,
  current_price    FLOAT DEFAULT NULL,
  last_updated     DATETIME,
  product_id       INT,
  shop_id          INT,
  PRIMARY KEY (product_price_id),
  KEY product_id (product_id),
  KEY shop_id (shop_id),
  CONSTRAINT product_conpp
    FOREIGN KEY (product_id) REFERENCES products (product_id),
  CONSTRAINT shop_conpp
    FOREIGN KEY (shop_id) REFERENCES shops (shop_id)
);

CREATE TABLE products_history (
  products_history_id INT NOT NULL AUTO_INCREMENT,
  price               FLOAT DEFAULT NULL,
  recorded_at         DATETIME,
  product_id          INT,
  shop_id             INT,
  PRIMARY KEY (products_history_id),
  KEY product_id (product_id),
  KEY shop_id (shop_id),
  CONSTRAINT product_conph
    FOREIGN KEY (product_id) REFERENCES products (product_id),
  CONSTRAINT shop_conph
    FOREIGN KEY (shop_id) REFERENCES shops (shop_id)
);

SET FOREIGN_KEY_CHECKS = 1;

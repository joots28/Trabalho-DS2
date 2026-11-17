#1. Criação do Banco de Dados
CREATE DATABASE IF NOT EXISTS db_seguradora;
USE db_seguradora;

#2. Tabela de Clientes
CREATE TABLE IF NOT EXISTS cliente (
    id_cliente INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    endereco VARCHAR(150) NOT NULL,
    telefone VARCHAR(20),
    email VARCHAR(100),
    data_nascimento DATE,
    profissao VARCHAR(50)
);

#3. Tabela de Automóveis
CREATE TABLE IF NOT EXISTS automovel (
    id_automovel INT AUTO_INCREMENT PRIMARY KEY,
    placa VARCHAR(10) NOT NULL,
    modelo VARCHAR(50) NOT NULL,
    marca VARCHAR(50) NOT NULL,
    ano INT,
    id_cliente INT NOT NULL,
    FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente) ON DELETE CASCADE
);

#4. Tabela de Seguros
CREATE TABLE IF NOT EXISTS seguro (
    id_seguro INT AUTO_INCREMENT PRIMARY KEY,
    numero_apolice VARCHAR(50) NOT NULL,
    valor_cobertura DECIMAL(10,2),
    id_cliente INT NOT NULL,
    id_automovel INT NOT NULL,
    FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente),
    FOREIGN KEY (id_automovel) REFERENCES automovel(id_automovel)
);

#5. Tabela de Acidentes
CREATE TABLE IF NOT EXISTS acidente (
    id_acidente INT AUTO_INCREMENT PRIMARY KEY,
    data_acidente DATE NOT NULL,
    hora_acidente TIME NOT NULL,
    endereco_acidente VARCHAR(150) NOT NULL,
    motivo_acidente TEXT,
    automoveis_envolvidos TEXT,
    danos_terceiros TEXT,
    id_automovel INT NOT NULL,
    FOREIGN KEY (id_automovel) REFERENCES automovel(id_automovel)
);

#6. Tabela de Pagamento do Seguro
CREATE TABLE IF NOT EXISTS pagamento (
    id_pagamento INT AUTO_INCREMENT PRIMARY KEY,
    valor DECIMAL(10,2) NOT NULL,
    data_pagamento DATE NOT NULL,
    id_cliente INT NOT NULL,
    id_acidente INT NOT NULL,
    FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente),
    FOREIGN KEY (id_acidente) REFERENCES acidente(id_acidente)
);
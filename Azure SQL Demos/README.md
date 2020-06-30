# Azure SQL Demos, Reference Architectures & Best Practices

This repository is meant to organize South East' Microsoft's Open Azure SQL based repositories for customer ready demos.

# Keywords
Azure, Azure SQL, Azure Elastic Scale, Azure Hyper Scale, Azure Synapse, Azure SQL Migration

## Table of contents
1. [Getting Started](#Getting-Started)
2. [Azure SQL Migration Demo](#azsqlmigrationdemo)
3. [Azure SQL Performance Tuning Demo](#azsqlperformancedemo)
4. [Azure SQL MI CLR Demo](#azsqlmiclrdemo)
3. [Azure SQL Database DB (Coming Soon)](#azsqldbdemo)
4. [Azure Elastic Scale Demo(Coming Soon)](#azelasticscaledemo)
5. [Azure Hyper Scale Demo(Coming Soon)](#azhyperscaledemo)
6. [Azure Synapse Demo(Coming Soon)](#azsynapsedemo)
7. [Azure SQL MI Demo(Coming Soon)](#azssqlmidemo)



# Getting Started <a name="Getting-Started"></a>
This repository is arranged as submodules so you can either pull all the tutorials or simply the ones you want. 
To pull all the tutorials run:


```bash
git clone --recurse-submodules https://github.com/microsoft/ai
```

if you have git older than 2.13 run:

```bash
git clone --recursive https://github.com/microsoft/ai.git
```

To pull a single submodule (e.g. DeployDeepModelKubernetes) run:
```
git clone https://github.com/microsoft/ai
cd ai
git submodule init submodules/DeployDeepModelKubernetes
git submodule update
```

# [Azure SQL Migration Demos](./azsql-migration-demos)<a name="azsqlmigrationdemo"></a>
Updated: 06/12/2020 <br>
I will demonstrate how to utilize the Data Migration Service (DMS) to Migrate from On-Premise SQL to the following Azure SQL PaaS Offerings:  

# [Azure SQL Performance Tuning Demo](./azsql-performance-tuning-demos)<a name="azsqlperformancedemo"></a>
Updated: 06/12/2020 <br>
These demos discuss the monitoring Azure SQL Databases using Metrics and diagnostics logging, Query performance Insight, Azure SQL Analytics for Advance performance Monitoring, and contrast this with On-premise vs PaaS approaches 

# [Azure SQL MI CLR Demo](./azsqlmi-clr-demo)<a name="azsqlmiclrdemo"></a>
Updated: 06/12/2020 <br>
This demo will cover how to deploy a CLR Assembly to SQL Managed Instance

# [Azure SQL Database DB (Coming Soon)](./azsql-migration-demos)<a name="azsqldbdemo"></a>

# [Azure Elastic Scale Demo(Coming Soon)](./azsql-migration-demos)<a name="azelasticscaledemo"></a>

# [Azure Hyper Scale Demo(Coming Soon)](./azsql-migration-demos)<a name="azhyperscaledemo"></a>

# [Azure Synapse Demo(Coming Soon)](./azsql-migration-demos)<a name="azsynapsedemo"></a>


# [Azure Synapse Demo(Coming Soon)](./azsql-migration-demos)<a name="azsynapsedemo"></a>

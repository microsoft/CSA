![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png "Microsoft Cloud Workshops")


<div class="MCWHeader1">
Migrate EDW to Azure SQL Data Warehouse
</div>

<div class="MCWHeader2">
Hands-on lab unguided
</div>

<div class="MCWHeader3">
September 2018
</div>


Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2018 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [Migrate EDW to Azure SQL Data Warehouse hands-on lab unguided](#migrate-edw-to-azure-sql-data-warehouse-hands-on-lab-unguided)
    - [Abstract and learning objectives](#abstract-and-learning-objectives)
    - [Overview](#overview)
    - [Solution architecture](#solution-architecture)
    - [Requirements](#requirements)
    - [Exercise 1: Configure Azure Services](#exercise-1-configure-azure-services)
        - [Task 1: Create a logical SQL Server to host SSISDB](#task-1-create-a-logical-sql-server-to-host-ssisdb)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 2: Create an Azure Data Factory v2](#task-2-create-an-azure-data-factory-v2)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 3: Create an Azure Storage Account](#task-3-create-an-azure-storage-account)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 4: Create an Azure SQL Data Warehouse](#task-4-create-an-azure-sql-data-warehouse)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 5: Create an Azure Analysis Services](#task-5-create-an-azure-analysis-services)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 6: Prepare Environment and Create Migration Accounts](#task-6-prepare-environment-and-create-migration-accounts)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
    - [Exercise 2: Data and schema preparation](#exercise-2-data-and-schema-preparation)
        - [Task 1: Validate schema and data](#task-1-validate-schema-and-data)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 2: Prepare Azure SQL Data Warehouse and migrate schema](#task-2-prepare-azure-sql-data-warehouse-and-migrate-schema)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
    - [Exercise 3: Migrate the data to Azure SQL Data Warehouse](#exercise-3-migrate-the-data-to-azure-sql-data-warehouse)
        - [Task 1: Export the data from your current data warehouse](#task-1-export-the-data-from-your-current-data-warehouse)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 2: Transfer your data to Azure](#task-2-transfer-your-data-to-azure)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
    - [Exercise 4: Migrate an SSIS Package to Data Factory v2](#exercise-4-migrate-an-ssis-package-to-data-factory-v2)
        - [Task 1: Deploy SSIS Package to Data Factory](#task-1-deploy-ssis-package-to-data-factory)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 2: Schedule the SSIS Package](#task-2-schedule-the-ssis-package)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
    - [Exercise 5: Create an Analysis Services Model](#exercise-5-create-an-analysis-services-model)
        - [Task 1: Configure Analysis Services backup](#task-1-configure-analysis-services-backup)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 2: Restore Analysis Services backup](#task-2-restore-analysis-services-backup)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 3: Update Analysis Services connections](#task-3-update-analysis-services-connections)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
    - [Exercise 6: Visualize data with Power BI Desktop](#exercise-6-visualize-data-with-power-bi-desktop)
        - [Task 1: Install Power BI Desktop](#task-1-install-power-bi-desktop)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
        - [Task 2: Query data with Power BI Desktop](#task-2-query-data-with-power-bi-desktop)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)
    - [After the hands-on lab](#after-the-hands-on-lab)
        - [Task 1: Cleanup resource groups](#task-1-cleanup-resource-groups)
            - [Tasks to complete](#tasks-to-complete)
            - [Exit criteria](#exit-criteria)

<!-- /TOC -->

# Migrate EDW to Azure SQL Data Warehouse hands-on lab unguided

## Abstract and learning objectives

In this hands-on lab you will migrate an existing on-premises enterprise data warehouse to the cloud. You will investigate the current data warehouse to identify any incompatibilities, export the data from the on-premises data warehouse, and transfer it to an Azure Blob Storage. You will then load the data into the warehouse using Polybase. Finally, you will integrate the warehouse by migrating ETL to Azure Data Factory and supporting ad-hoc access by implementing Azure Analysis Services. 

At the end of this hands-on lab, you will be better able to plan and implement a migration of your existing on-premises enterprise data warehouse to Azure SQL Data Warehouse and integrating it with both cloud-based and on-premises services and data sources.

## Overview

Coho, a retail company focusing on consumer electronics, is modernizing their data architecture. Critical to this effort is migrating their existing enterprise data warehouse to the cloud for better integration with their cloud native customer 360 project and self-service business intelligence for their people in the field.

## Solution architecture

![An image of the solution architecture that depicts a simulated on-premises environment hosting SQL Server with paths for data to be uploaded to Azure Blob Storage and schema transferred to Azure SQL Datawarehouse. Finally, data is shown transferred to a computer running Power BI.](images/Hands-onlabunguided-MigrateEDWtoAzureSQLDataWarehouseimages/media/image2.png)

## Requirements

1.  Microsoft Azure Subscription
2.  Complete the Before the hands-on lab setup guide to deploy the \"on-premises\" environment.

## Exercise 1: Configure Azure Services

In this exercise, you will create and configure an Azure Storage Account, Azure SQL Data Warehouse, Azure Analysis Services and Azure Data Factory V2.

### Task 1: Create a logical SQL Server to host SSISDB

#### Tasks to complete

1.  Create a logical SQL Server to host the SSISDB database used by the SSIS Integrated Runtime.

#### Exit criteria

-   A logical SQL Server deployed to one of the regions supported by the Integration Runtime.

### Task 2: Create an Azure Data Factory v2

#### Tasks to complete

1.  Create a Data Factory v2.
2.  Configure the SSIS Integrated Runtime.

#### Exit criteria

-   An Azure Data Factory v2 instance configured to use the SSIS Integration Runtime.

### Task 3: Create an Azure Storage Account

#### Tasks to complete

1.  Create an Azure Storage Account in the same region as your SQL Server to be used during the data migration.

2.  Create a blob container in the storage account.

3.  The resource group should be EDWmigrationStor.

#### Exit criteria

-   A Storage Account in the same region as your SQL Server virtual machine with a container to hold data being migrated.

### Task 4: Create an Azure SQL Data Warehouse

#### Tasks to complete

1.  Create an Azure SQL Data Warehouse in the same region as your SQL Server to be used during the data migration.

2.  The resource group should be CohoDWRG.

#### Exit criteria

-   An Azure SQL Data Warehouse in the same region as your SQL Server.

### Task 5: Create an Azure Analysis Services

#### Tasks to complete

1.  Create an Azure Analysis Services in the same region and resource group as your SQL Data Warehouse.

#### Exit criteria

-   An Azure SQL Data Warehouse in the same region as your SQL Server.

### Task 6: Prepare Environment and Create Migration Accounts

#### Tasks to complete

1.  Open firewall ports on all SQL Databases and Azure SQL Data Warehouse to allow access to the SQLcohoDW virtual machine.

1.  Use the **C:\\LabFiles\\CreateDataLoader.sql** script to create a data loader account that uses the large resource class on your Azure SQL Data Warehouse.

#### Exit criteria

-   Firewall ports open for SQLcohoDW on all SQL Databases and Azure SQL Data Warehouse.

-   User account that in largerc resource class on your Azure SQL Data Warehouse.

## Exercise 2: Data and schema preparation

Coho is relying on you to migrate the data warehouse to Azure SQL Data Warehouse. One of the most important steps is preparing the data and schema. During this phase, you will need to verify compatibility of the schema and data, and make any necessary changes required for a successful migration.

### Task 1: Validate schema and data

#### Tasks to complete

1.  Validate the CohoDW schema has no data incompatibility or data length issues.

2.  Script out any tables with potential issues.

3.  Resolve any data length issues you find in the table definition by verifying you will not truncate any data and altering the table.

#### Exit criteria

-   All schema incompatibilities, and data length issues have been addressed.

### Task 2: Prepare Azure SQL Data Warehouse and migrate schema

#### Tasks to complete

1.  Script out all tables in CohoDW.

2.  Correct any code that is not compatible with SQL Data Warehouse.

3.  Create the tables in Azure SQL Data Warehouse.

#### Exit criteria

-   All 33 tables successfully created in the SQL Data Warehouse.

## Exercise 3: Migrate the data to Azure SQL Data Warehouse

This exercise is focused on migrating the data from your existing data warehouse into SQL Data Warehouse. We will be pulling the data and uploading it to an Azure storage account. We will then import the data via Polybase.

### Task 1: Export the data from your current data warehouse

#### Tasks to complete

1.  Using the **bcp\_commands.txt** file, export the data from the tables of your data warehouse to the C:\\Migration folder.

#### Exit criteria

-   All 33 tables must have corresponding output files.

### Task 2: Transfer your data to Azure

#### Tasks to complete

1.  Download and install the Microsoft Azure Storage Tools from <http://aka.ms/downloadazcopy>.

2.  Use AzCopy to copy your output files to the migration container in your Azure Storage Account.

3.  Create the necessary objects required by Polybase to import the files.

4.  Use the **CreateExternalTables.sql** file to create the external table definitions.

5.  Use the **LoadData.sql** file to import the data into Azure SQL Data Warehouse.

#### Exit criteria

-   All 33 tables successfully copied to Azure Storage.

-   Correctly configured objects to support Polybase (master key, database scoped credential, external data source, external file format).

-   33 external tables created to support a Polybase data load operation.

-   Data loaded into the 33 tables in the SQL Data Warehouse.

## Exercise 4: Migrate an SSIS Package to Data Factory v2

In this exercise, you will use the SSIS Integration Runtime in Azure Data Factory to run a pre-built SSIS package. You will start by deploying a pre-built package into your SSISDB Catalog. You will then schedule that package via Azure Data Factory.

### Task 1: Deploy SSIS Package to Data Factory

#### Tasks to complete

1.  Deploy **C:\\LabFiles\\DataLoad.ispac** SSIS project to Data Factory v2.

2.  Reconfigure the package connection strings to point to your cohoOLTP database as a source and your SQL Data Warehouse as a destination.

#### Exit criteria

-   The DataLoad.ispac SSIS project should be deployed.

-   The package should run successfully without error.

### Task 2: Schedule the SSIS Package

#### Tasks to complete

1.  Create a pipeline to run the SSIS package.

2.  Configure the pipeline to run every 15 minutes.

#### Exit criteria

-   The pipeline executes the package without error.

-   The pipeline is scheduled to run every 15 minutes.

## Exercise 5: Create an Analysis Services Model

Coho has provided you with an existing Analysis Services model for use with the Data Warehouse. They have asked you to use this model to support ad-hoc query access from Power BI.

In this exercise, you will configure backup and restore for Analysis Services and create a tabular model to allow ad-hoc queries from client tools.

### Task 1: Configure Analysis Services backup

#### Tasks to complete

1.  Configure your Analysis Services instance to enable backup/restore to a new storage account.

#### Exit criteria

-   Analysis Services backup correct configured to backup/restore from a new storage account.

### Task 2: Restore Analysis Services backup

#### Tasks to complete

1.  Restore the **coho-data-model.abf** data model to your Analysis Services instance.

#### Exit criteria

-   Coho-data-model is restored an online.

### Task 3: Update Analysis Services connections

#### Tasks to complete

1.  Update the connection in coho-data-model to point to your Azure SQL Data Warehouse.

2.  Process the coho-data-model database.

#### Exit criteria

-   Coho-data-model is able to connect to the Azure SQL Data Warehouse.

-   The coho-data-model has been processed.

## Exercise 6: Visualize data with Power BI Desktop

In this exercise, you will setup integration with Power BI Desktop.

### Task 1: Install Power BI Desktop

#### Tasks to complete

1.  Install Power BI Desktop on your SQLcohoDW virtual machine.

#### Exit criteria

-   Power BI Desktop installed on SQLcohoDW.

### Task 2: Query data with Power BI Desktop

#### Tasks to complete

1.  Connect Power BI to your Analysis Services instance.

2.  Create a report that shows the total sales by country/region on map.

#### Exit criteria

-   Power BI report that shows the total sales by country/region on a map.

## After the hands-on lab 

To prevent excessive charges, you should cleanup the resources you have created for this lab.

### Task 1: Cleanup resource groups

#### Tasks to complete

1.  Delete the **CohoDWRG, EDWmigrationStor** and **OnPremEnvironment** resource groups.

#### Exit criteria

-   No resources created during this lab should remain.

You should follow all steps provided *after* attending the hands-on lab.


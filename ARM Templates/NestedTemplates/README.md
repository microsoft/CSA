# Nested Teamplates  
The Nested Templates are built to be generic in nature with the ability to pass in the values needed to build your specific architecture. Below you will find details on how to utilize each of the nested templates in this folder:  

### Table of Contents

* [Shared Services Templates](#SharedTemplates)
     - [VNet](#VnetTemplate)  
     - [Empty NSG](#NSG-EMPTY-ExistingSubnetTemplate)  
     - [NSG with Rules](#NSG-ExistingSubnetTemplate)   
     - [Public IP Address](#PublicIPAddressTemplate) 
     - [User Assigned Managed Identity](#UserAssignedManagedIdentityTemplate)  
     - [Application Insights Instance](#ApplicationInsightsTemplate)  
     - [Log Analytics Workspace](#LogAnalyticsWorkspace) 
     - [Key Vault](#KeyVault)  
     - [Key Vault Secret](#KeyVaultSecret)  
     - [Add Access Policy to Key Vault](#KeyVaultAccessPolicyTemplate) 
     - [Azure Bastion Host](#AzureBastion)
     - [Azure Bastion Host](#AzureBastion) 
     - [Get Nic IP](#GetNicIP)  
* [IaaS Templates](#IaaSTemplates)
     - [Ubuntu Virtual Machine](#UbuntuVirtualMachine)
     - [Enable VM Insights](#EnabledVMInsights) 
* [Load Balancer Templates](#LoadBalancerTemplates)
     - [Application Gateway with HTTP Listener](#AppGWHTTPListenerTemplate)  
     - [AppGW with HTTPS Listener Template and Key Vault Integration](#AppGWHTTPSListenerKVTemplate) 
* [Container Templates](#ContainerTemplates)  
     - [Azure Container Registry](#AzureContainerRegistry)  
     - [AKS Cluster](#AKSCluster)
* [Web Management](#WebManagement)
     - [Azure API Management](#APIM)   
     - [Azure Redis Cache VNet Injection](#AzureRedisCacheVnet)  


# <a name="SharedTemplates"></a>Shared Service Template  

## <a name="VnetTemplate"></a>VNet Template  
This template will deploy a Virtual Network in Azure. It accepts a dynamic list of Subnets with their IP Ranges.  

### Typical Nested Template used before  
NA  

### Typical Nested Template used after  
NSG-Empty-ExistingSubnet  
NSG-ExistingSubnet  

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  vNETName | Name of the Virtual Network   | pocVNET    |
| addressRange   | Address Range the the entire subnet | 10.0.0.0/16  |
| subnets   | Array of subnets with their IP Range. The subnet range should be seperated from the IP Range with the \| deliminator | ["subnetA\|10.0.1.0/24","subnetB\|10.0.2.0/24","subnetC\|10.0.3.0/24"]  |  

### Output
"vnetId": The resource id of the VNet created

### Sample Deployment  

    {
      "name": "deployVNET",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployVNETTemplateURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "vNETName": {
            "value": "pocVNET"
          },
          "addressRange": {
            "value": "10.0.0.0/16"
          },
          "subnets": {
            "value": [
                "subnetA\|10.0.1.0/24",
                "subnetB\|10.0.2.0/24",
                "subnetC\|10.0.3.0/24"
            ]
          }
        }
      }
    }  

## <a name="NSG-EMPTY-ExistingSubnetTemplate"></a>NSG-EMPTY-ExistingSubnet Template   
This template will deploy an empty NSG and attach it to an exising Subnet  

### Typical Nested Template used before  
VNet  

### Typical Nested Template used after  
WindowsVirtualMachine  
LinuxVirtualMachine  
APIM  
AppGW  
AzureRedisCache  
PrivateAKSMICluster  
PrivateEndpoint    

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  virtualNetworkName | Name of the existing Virtual Network   | pocVNET    |
| subnetName   | Name of the subnet in the existing VNet to attach the NSG to | subnetA  |
| subnetAddressPrefix   | The IP range of the subnet attaching the NSG to | 10.0.1.0/24  |
| nsgName   | Name of the NSG | subnetA-NSG  |  

### Output
na

### Sample Deployment  

    {
      "name": "deployAKSNSG",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployVNET"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployNSGEmptyTemplateURL')]",
          "contentVersion": "1.0.0.1"
        },
        "parameters": {
          "virtualNetworkName": {
            "value": "pocVNET"
          },
          "subnetName": {
            "value": "subnetA"
          },
          "subnetAddressPrefix": {
            "value": "[reference(resourceId(resourceGroup().name, 'Microsoft.Network/virtualNetworks/subnets', 'pocVNEt', 'subnetA'), '2018-03-01').addressPrefix]"
          },
          "nsgName": {
            "value": "subnetA-NSG"
          }
        }
      }
    }


## <a name="NSG-ExistingSubnetTemplate"></a>NSG-ExistingSubnet Template   
This template will deploy an NSG and attach it to an exising Subnet. You pass in the NSG rules for the NSG using an array. 

### Typical Nested Template used before  
VNet  

### Typical Nested Template used after  
WindowsVirtualMachine  
LinuxVirtualMachine  
APIM  
AppGW  
AzureRedisCache  
PrivateAKSMICluster  
PrivateEndpoint    

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  virtualNetworkName | Name of the existing Virtual Network   | pocVNET    |
| subnetName   | Name of the subnet in the existing VNet to attach the NSG to | subnetA  |
| subnetAddressPrefix   | The IP range of the subnet attaching the NSG to | 10.0.1.0/24  |
| nsgName   | Name of the NSG | subnetA-NSG  |  
| securityRules   | Array of security rules with a \| deleminator | RuleName\|Description\|Protocol\|Source Port Range\|Destination Port Range\|Source Address Prefix\|Destination Address Prefix\|Access\|Priority\|Direction  |  

### Output
na


### Sample Deployment  

    {
      "name": "deployAKSNSG",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployVNET"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployNSGTemplateURL')]",
          "contentVersion": "1.0.0.1"
        },
        "parameters": {
          "virtualNetworkName": {
            "value": "pocVNET"
          },
          "subnetName": {
            "value": "subnetB"
          },
          "subnetAddressPrefix": {
            "value": "[reference(resourceId(resourceGroup().name, 'Microsoft.Network/virtualNetworks/subnets', 'pocVNEt', 'subnetB'), '2018-03-01').addressPrefix]"
          },
          "nsgName": {
            "value": "subnetB-NSG"
          },
          "securityRules": {
            "value": [
              "deny-all|Deny-All-Traffic|Tcp|*|*|*|*|Deny|500|Inbound",
              "allow-443|Allow-SSL|Tcp|*|443|*|*|Allow|100|Inbound",
              "allow-8080|Allow-SSL|Tcp|*|8080|*|*|Allow|110|Inbound",
              "allow-HealthProbe|Allow-AppGWHealth|Tcp|*|65200-65535|*|*|Allow|120|Inbound"
            ]
          }
        }
      }
    }

## <a name="PublicIPAddressTemplate"></a>Public IP Address Template   
This template will deploy standard sku Public IP Address

### Typical Nested Template used before  
NA

### Typical Nested Template used after  
WindowsVirtualMachine  
LinuxVirtualMachine  
AppGW  

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
| publicIpAddressName | Name of the public IP   | poc-pip    |
| sku   | SKU for the Public IP. Either basic or standard | Standard  |
| allocationMethod   | Static or Dynamic allocation of IP | Static  |  

### Output  
"publicIPID": The resource id of the public ip created 


### Sample Deployment  

    {
      "name": "deployPublicIP1",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployPublicIPTemplateURL')]",
          "contentVersion": "1.0.0.1"
        },
        "parameters": {
          "publicIpAddressName": {
            "value": "pocpip"
          },
          "sku": {
              "value": "Standard"
          },
          "allocationMethod": {
              "value": "Static"
          }
        }
      }
    }

## <a name="UserAssignedManagedIdentityTemplate"></a>User Assigned Managed Identity Template  
This template will create a User Assigned Managed Identity.   

### Typical Nested Template used before  
NA  

### Typical Nested Template used after  
AppGWHTTPSListenerKV  
WindowsVirtualMachine  
LinuxVirtualMachine  

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  identityName | Name of the identity to be created   | pocGW-Identity    |

### Output  
"principalId": The principal ID of the Managed Identity that was created  
"resourceId": The resource id of the Managed Identity that was created  

### Sample Deployment  

    {
      "name": "createManagedIdentity",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "resourceGroup": "[parameters('resourceGroup')]",
      "dependsOn": [
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('createManagedIdentityTemplateURL')]",
          "contentVersion": "1.0.0.1"
        },
        "parameters": {
          "identityName": {
              "value": "[concat(parameters('applicationGatewayName'),'-identity')]"
          }
        }
      }
    }  
  

## <a name="ApplicationInsightsTemplate"></a>Application Insights Template  
This template will create an Application Insights.   

### Typical Nested Template used before  
NA  

### Typical Nested Template used after  
APIM  

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  name | Application Insights instance name   | poc-appinsights    |

### Output  
"appInsightsID": The resource id of the Application Insights instance that was created  

### Sample Deployment  

    {
      "name": "deployAppInsights",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployAppInsightsTemplateURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "name": {
            "value": "[parameters('appInsightsName')]"
          }
        }
      }
    }  

## <a name="LogAnalyticsWorkspace"></a>Log Analytics Workspace Template  
This template will create a Log Analytics Workspace.   

### Typical Nested Template used before  
NA  

### Typical Nested Template used after  
VMInsights  
DiagnosticSettings    

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  workspaceName | Log Analytics workspace name   | poc-laworkspace    |

### Output  
"workspaceId": The resource id of the Log Analytics Workspace that was created  
"workspaceKey": The primary key for the workspace
"customerId": The customer id for the workspace  

### Sample Deployment  

    {
      "name": "deployLAWorkspace",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployLogAnalyticsURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "workspaceName": {
            "value": "[parameters('workspaceName')]"
          }
        }
      }
    }

## <a name="KeyVault"></a>Key Vault Template  
This template will deploy an Azure Key Vault   

### Typical Nested Template used before  
NA     

### Typical Nested Template used after  
KeyVaultAccessPolicy  
KeyVaultSecrets      

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  vaultName | Key Vault name   | poc-keyvault    |

### Output  
"vaultId": The resource id of the Key Vault that was created   

### Sample Deployment  

    {
      "name": "deployKeyVault",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployLAWorkspace"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployKeyVaultURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "vaultName": {
            "value": "[parameters('vaultName')]"
          },
          "workspaceID": {
            "value": "[reference('deployLAWorkspace').outputs.workspaceId.value]"
          }
        }
      }
    }

## <a name="KeyVaultSecret"></a>Key Vault Secret Template  
This template will add a secret to an Azure Key Vault   

### Typical Nested Template used before  
KeyVault     

### Typical Nested Template used after  
NA        

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  vaultName | Key Vault name   | poc-keyvault    |
|  secretName | Secret name to add to Key Vault   | supersecret    |   
|  contentType | Type of data being added to secret   | text/plain    |  
|  value | Value of the secret being added   | supersecretvalue    |  

### Output  
NA  

### Sample Deployment  

    {
      "name": "addKeyVaultSecret",
      "type": "Microsoft.Resources/deployments",
      "resourceGroup": "[parameters('keyVaultResourceGroup')]",
      "apiVersion": "2017-05-10",
      "dependsOn": [
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('addKeyVaultSecretTemplate')]",
          "contentVersion": "1.0.0.1"
        },
        "parameters": {
          "keyVaultName": {
              "value": "[parameters('keyVaultName')]"
          },
          "secretName": {
            "value": "[parameters('keyVaultSecretName')]"
          },
          "contentType": {
              "value": "[parameters('keyVaultContentType')]"
          },
          "value": {
              "value": "[parameters('keyVaultSecretValue')]"
          }
        }
      }
    }

## <a name="KeyVaultAccessPolicyTemplate"></a>Key Vault Access Policy Template  
This template will create an acces policy to secrets in an existing Key Vault. It is currently limited to grating rights to secrets.   

## <a name="AzureBastion"></a>Azure Bastion Template  
This template will deploy an Azure Bastion to an existing VNET 

### Typ
Vnet  
PublicIPAddress  

### Typical Nested Template used after  
WindowsVirtualMachine  
LinuxVirtualMachine             

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  bastionHostName | Name for the bastion host  | poc-bastionhost    |
|  subnetId | SubnetID of the subnet dedicated to Azure Bastion  | [concat(reference('deployVNET').outputs.vnetId.value,'/subnets/AzureBastionSubnet')]    |
|  publicIpId | Resource ID of the public ip for the bastion host  | [reference('deployPublicIPBastion').outputs.publicIPID.value]   |  

### Output  
NA 

### Sample Deployment  

    {
      "name": "deployPublicIPBastion",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "resourceGroup": "[parameters('resourceGroup')]",
      "dependsOn": [],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployPublicIPTemplateURL')]",
          "contentVersion": "1.0.0.1"
        },
        "parameters": {
          "publicIpAddressName": {
            "value": "[concat(parameters('bastionHostName'),'pip1')]"
          },
          "sku": {
              "value": "Standard"
          },
          "allocationMethod": {
              "value": "Static"
          }
        }
      }
    },
    {
      "name": "deployAzureBastion",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployVNET"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployAzureBastionTemplateURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "bastionHostName": {
              "value": "[parameters('bastionHostName')]"
          },
          "subnetId": {
              "value": "[concat(reference('deployVNET').outputs.vnetId.value,'/subnets/AzureBastionSubnet')]"
          },
          "publicIpId": {
              "value": "[reference('deployPublicIPBastion').outputs.publicIPID.value]"
          }
        }
      }
    }

## <a name="GetNicIP"></a>Get Network Interface IP Template  
This template will return the first IP address assigned to a Azure Network Interface. This can be used to get an IP so you can add it to a Private DNS Zone    

### Typical Neted Template used before
PrivateEndpoint  
WindowsVirtualMachine  
LinuxVirtualMachine      

### Typical Nested Template used after  
PrivateDNSARecord             

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  nicID | ResourceId for the network interface  | [reference('deploySqlServerPE').outputs.nicID.value]    |

### Output  
"nicIP": IP Address of the NIC

### Sample Deployment  

    {
      "name": "getSqlServerNICIP",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deploySqlServerPE"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('getNICIPUrL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "nicID": {
            "value": "[reference('deploySqlServerPE').outputs.nicID.value]"
          }
        }
      }
    }

# <a name=IaaSTemplates"></a>IaaS Templates

## <a name="UbuntuVirtualMachine"></a>Ubuntu Virtual Machine    
This template will create a Ubuntu Virtual Machine.      

### Typical Neted Template used before
VNet          

### Typical Nested Template used after  
EnableVMInsights              

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  subnetName | Subnet name where the nic will be placed  | shared-SN    |
|  virtualNetworkId | VNet ID where the nic will be placed  | [reference('deployVNet').outputs.vnetID.value]    |  
|  virtualMachineName | Name of the Virtual Machine  | pocVM    |  
|  ubuntuOSVersion | Allowd values: 18.04-LTS, 16.04-LTS, 14.04.4-LTS  | 18.04-LTS    |  
|  adminUsername | Administrator username  | LinuxAdmin    |  
|  adminPassword | Administrator password  | ABCabc1234    |  
|  zone | Availability zone to place the VM  | 1    |

### Output  
"vmID": Resource id of the virtual machine created  
"nicID": Resource id of the nic created

### Sample Deployment  

    {
      "name": "deployUbuntuBox",
      "comments":"NOTE: OS and Datadisks cannot be tagged when provisioned within VM.  Would need to provision DISK with tags first, then reference",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployVNET"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployUbuntuServerTemplateURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "subnetID": {
            "value": "[reference('deployVNET').outputs.sharedSubnetID.value]"
          },
          "virtualMachineName": {
            "value": "[parameters('ubuntuName')]"
          },
          "virtualMachineSize": {
            "value": "[parameters('ubuntuSize')]"
          },
          "adminUsername": {
            "value": "[parameters('adminUserName')]"
          },
          "adminPassword": {
            "value": "[parameters('adminPassword')]"
          },
          "ubuntuOSVersion": {
            "value": "18.04-LTS"
          },
          "zone": {
            "value": "1"
          }
        }
      }
    }  

## <a name="EnableVMInsights"></a>Enabled VM Insights on an Existing VM Template  
This template will enable VM Insights (Azure Monitor for VMs) to an existing VM  

### Typical Neted Template used before
Log_Analytics_Workspace  
WindowsVirtualMachine  
LinuxVirtualMachine      

### Typical Nested Template used after  
NA             

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  vmResourceId | ResourceId for the VM to be monitored  | [reference('deployJumpBox').outputs.vmID.value]    |
|  osType | Linux or Windows  | Windows    |
|  workspaceResourceId | Resource ID of the log analytics workspace to use  | [reference('deployLAWorkspace').outputs.workspaceId.value]   |

### Output  
NA 

### Sample Deployment  

    {
      "name": "addJumpBoxInsights",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployJumpBox"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('addVMInsightsURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "VmResourceId": {
            "value": "[reference('deployJumpBox').outputs.vmID.value]"
          },
          "osType": {
            "value": "Windows"
          },
          "WorkspaceResourceId": {
            "value": "[reference('deployLAWorkspace').outputs.workspaceId.value]"
          }
        }
      }
    }

# <a name=LoadBalancerTemplates></a>Load Balancer Templates  

## <a name="AppGWHTTPListenerTemplate"></a>AppGW with HTTP Listener Template  
This template will deploy an Application Gateway with a HTTP Listeners and Basic routing rules. Note this does not deploy path based routing or a private IP listener  

### Typical Nested Template used before  
PublicIPAddress  

### Typical Nested Template used after  
NA  

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
| applicationGatewayName | Name of the Application Gateway   | pocAppGW    |
| tier   | Standard, WAF, Standard_v2, WAF_v2 | WAF_v2  |
| skuSize   | Name of an application gateway SKU. - Standard_Small, Standard_Medium, Standard_Large, WAF_Medium, WAF_Large, Standard_v2, WAF_v2 | WAF_v2  | 
| minCapacity   | Min number of AppGW Capacity | 2  | 
| maxCapacity   | Min number of AppGW Capacity |  4 |
| zones   | The Availability Zones the AppGW can be scaled to | ["1","2","3"]  |
| subnetID   | Resource ID of the subnet the AppGW will sit on | "[concat(reference('deployVNET').outputs.vnetId.value,'/subnets/AppGW-SN')]"  |
| publicIpAddressesId  | Public IP for the Frontend. Format: name\|publicIP Resource ID  | [<br>"[concat('PIP1\|',reference('deployPublicIP1').outputs.publicIPID.value )]"<br>]  |
| frontendPorts | Ports that the AppGW will listen on. Format: name\|port | HTTP80\|80  |
| backendAddresses | The backend pools for the AppGW. Format: name\|backend IP or URL | [<br>"Example1\|example.com",<br>"Example2\|example2.com" <br>]  |
| backendHttpSettings | The HTTP Setting for the backend pool. Format: name\|port\|protocol\|cookieBasedAffinity\|RequestTimeout\|path  | [<br>"Example-App-HTTPSetting\|80\|Http\|Disabled\|30\|/ <br>]  |
| httpListeners | The HTTP Listener Settings for the frontend ip: Format: name\|fronte ip config name\|frontend port name | [<br>"Example1-App-Listener/|PIP1/|HTTP-80", <br>"Example2-App-Listener/|PIP1/|HTTP-8080"<br>]  |
| requestRoutingRules  | Routing rules for the AppGW. Format: name\|httpListener name\|backend pool name\|backend http setting name | [<br>"Example1-App-RoutingRule\|Example1-App-Listener\|Example1-BEPool\|Example-App-HTTPSetting",  <br>"Example2-App-RoutingRule\|Example2-App-Listener\|Example2-BEPool\|Example-App-HTTPSetting"<br>]  |  

### Output  
na

### Sample Deployment  

    {
      "name": "deployAppGW",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployVNET",
        "deployPublicIP1"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployAppGWHTTPListenerTemplateURL')]",
          "contentVersion": "1.0.0.1"
        },
        "parameters": {
          "applicationGatewayName": {
              "value": "[parameters('applicationGatewayName')]"
          },
          "tier": {
              "value": "[parameters('appgwtier')]"
          },
          "skuSize": {
              "value": "[parameters('appgwskuSize')]"
          },
          "minCapacity": {
              "value": "[parameters('appgwMinCapacity')]",
          },
          "maxCapacity": {
              "value": "[parameters('appgwMaxCapacity')]",
          },
          "zones": {
              "value": "[parameters('appgwzones')]"
          },
          "subnetID": {
              "value": "[concat(reference('deployVNET').outputs.vnetId.value,'/subnets/AppGW-SN')]"
          },
          "publicIpAddressesIds": {
              "value": [
                "[concat('PIP1|',reference('deployPublicIP1').outputs.publicIPID.value )]"
              ]
          },
          "frontendPorts": {
              "value": [
                "HTTP-80|80",
                "HTTP-8080|8080"
              ]
          },
          "backendAddresses": {
              "value": [
                "Example1-BEPool|bing.com",
                "Example2-BEPool|microsoft.com"
              ]
          },
          "backendHttpSettings": {
              "value": [
                "Example-App-HTTPSetting|80|Http|Disabled|30|/"
              ]
          },
          "httpListeners": {
              "value": [
                "Example1-App-Listener|PIP1|HTTP-80",
                "Example2-App-Listener|PIP1|HTTP-8080"
              ]
          },
          "requestRoutingRules": {
              "value": [
                "Example1-App-RoutingRule|Example1-App-Listener|Example1-BEPool|Example-App-HTTPSetting",
                "Example2-App-RoutingRule|Example2-App-Listener|Example2-BEPool|Example-App-HTTPSetting"
              ]
          }
        }
      }
    }

## <a name="AppGWHTTPSListenerKVTemplate"></a>AppGW with HTTPS Listener Template and Key Vault Integration  
This template will deploy an Application Gateway with a HTTPS Listeners and Basic routing rules. This will pull the template from an existing key vault with the certificate uploaded. The template make the following assumptions:  
1) There will be a different certificate for each HTTP Listener
2) There will be only a single certificate for each HTTP Listener  
3) The order of the certificate parameter will match the order you want them applied ot the HTTP Listeners

### Typical Nested Template used before  
PublicIPAddress  
ManagedIdentity

### Typical Nested Template used after  
NA  

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
| applicationGatewayName | Name of the Application Gateway   | pocAppGW    |
| tier   | Standard, WAF, Standard_v2, WAF_v2 | WAF_v2  |
| skuSize   | Name of an application gateway SKU. - Standard_Small, Standard_Medium, Standard_Large, WAF_Medium, WAF_Large, Standard_v2, WAF_v2 | WAF_v2  | 
| minCapacity   | Min number of AppGW Capacity | 2  | 
| maxCapacity   | Min number of AppGW Capacity |  4 |
| zones   | The Availability Zones the AppGW can be scaled to | ["1","2","3"]  |
| subnetID   | Resource ID of the subnet the AppGW will sit on | "[concat(reference('deployVNET').outputs.vnetId.value,'/subnets/AppGW-SN')]"  |  
| keyVaultName   | The name of the Key Vault that contains the SSL certificates | pocKeyVault  |  
| identityID   | The user Assigned Managed Identity resource ID that will be attahed to the GW and used to pull the certificates | poc-identity  |  
| certificates   | Reference to the certificates in the Key Vault. Format: Cert Name in AppGW\|Path in KeyVault | [<br>"ARMCert\|ARM/df47f485ecb1455d98eae9a950af6f47",<br>"ARM2Cert\|ARM2/b5e5e9c31a034bffa8387ef38754333f"<br>]  |
| publicIpAddressesId  | Public IP for the Frontend. Format: name\|publicIP Resource ID  | [<br>"[concat('PIP1\|',reference('deployPublicIP1').outputs.publicIPID.value )]"<br>]  |
| frontendPorts | Ports that the AppGW will listen on. Format: name\|port | HTTP80\|80  |
| backendAddresses | The backend pools for the AppGW. Format: name\|backend IP or URL | [<br>"Example1\|example.com",<br>"Example2\|example2.com" <br>]  |
| backendHttpSettings | The HTTP Setting for the backend pool. Format: name\|port\|protocol\|cookieBasedAffinity\|RequestTimeout\|path  | [<br>"Example-App-HTTPSetting\|80\|Http\|Disabled\|30\|/ <br>]  |
| httpListeners | The HTTP Listener Settings for the frontend ip: Format: name\|fronte ip config name\|frontend port name | [<br>"Example1-App-Listener/|PIP1/|HTTP-80", <br>"Example2-App-Listener/|PIP1/|HTTP-8080"<br>]  |
| requestRoutingRules  | Routing rules for the AppGW. Format: name\|httpListener name\|backend pool name\|backend http setting name | [<br>"Example1-App-RoutingRule\|Example1-App-Listener\|Example1-BEPool\|Example-App-HTTPSetting",  <br>"Example2-App-RoutingRule\|Example2-App-Listener\|Example2-BEPool\|Example-App-HTTPSetting"<br>]  |  

### Output  
na

### Sample Deployment  

    {
      "name": "deployAppGW",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "resourceGroup": "[parameters('resourceGroup')]",
      "dependsOn": [
        "deployVNET",
        "deployPublicIP1"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployAppGWHTTPSListenerKVTemplateURL')]",
          "contentVersion": "1.0.0.1"
        },
        "parameters": {
          "applicationGatewayName": {
              "value": "[parameters('applicationGatewayName')]"
          },
          "tier": {
              "value": "[parameters('appgwtier')]"
          },
          "skuSize": {
              "value": "[parameters('appgwskuSize')]"
          },
          "minCapacity": {
              "value": "[parameters('appgwMinCapacity')]",
          },
          "maxCapacity": {
              "value": "[parameters('appgwMaxCapacity')]",
          },
          "zones": {
              "value": "[parameters('appgwzones')]"
          },
          "subnetID": {
              "value": "[concat(reference('deployVNET').outputs.vnetId.value,'/subnets/AppGW-SN')]"
          },
          "publicIpAddressesIds": {
              "value": [
                "[concat('PIP1|',reference('deployPublicIP1').outputs.publicIPID.value )]"
              ]
          },
          "keyVaultName": {
            "value": "[parameters('keyVaultName')]"
          },
          "identityID": {
              "value": "[reference('createManagedIdentity').outputs.resourceId.value]"
          },
          "certificates": {
              "value": "[parameters('certificates')]"
          },
          "frontendPorts": {
              "value": [
                "HTTPS-443|443",
                "HTTPS-8080|8080"
              ]
          },
          "backendAddresses": {
              "value": [
                "Example1-BEPool|bing.com",
                "Example2-BEPool|microsoft.com"
              ]
          },
          "backendHttpSettings": {
              "value": [
                "Example-App-HTTPSetting|80|Http|Disabled|30|/"
              ]
          },
          "httpListeners": {
              "value": [
                "Example1-App-Listener|PIP1|HTTPS-443",
                "Example2-App-Listener|PIP1|HTTPS-8080"
              ]
          },
          "requestRoutingRules": {
              "value": [
                "Example1-App-RoutingRule|Example1-App-Listener|Example1-BEPool|Example-App-HTTPSetting",
                "Example2-App-RoutingRule|Example2-App-Listener|Example2-BEPool|Example-App-HTTPSetting"
              ]
          }
        }
      }
    }




### Typical Nested Template used before  
KeyVault  
UserAssignedManagedIdentity   

### Typical Nested Template used after  
AppGWHTTPSListenerKV  

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  keyVaultName | Name of the Key Vault to add the access polity   | poc-keyvault    |
|  secrets | This is an array of the rights given to access secrets   | [  "get",  "list", "set"  ]    |
|  objectId | This object id you want the rights granted to   | "[reference('createManagedIdentity').outputs.principalId.value]"    |

### Output  
NA

### Sample Deployment  

    {
      "name": "deployKeyVaultAccess",
      "type": "Microsoft.Resources/deployments",
      "resourceGroup": "[parameters('keyVaultResourceGroup')]",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "createManagedIdentity"
      ],
      "properties": {
          "mode": "Incremental",
          "templateLink": {
          "uri": "[variables('deployKeyVaultAccessTemplate')]",
          "contentVersion": "1.0.0.1"
          },
          "parameters": {
              "keyVaultName": {
                  "value": "[parameters('keyVaultName')]"
              },
              "secrets": {
                  "value": [
                      "Get",
                      "List",
                      "Set",
                      "Delete",
                      "Recover",
                      "Backup",
                      "Restore"
                  ]
              },
              "objectId": {
                  "value": "[reference('createManagedIdentity').outputs.principalId.value]"
              }
          }
      }
    }


## <a name="PrivateAKSCluster"></a>Private AKS Cluster    
This template will AKS cluster with Linux nodes.          

### Typical Neted Template used before
VNet          

### Typical Nested Template used after  
NA            

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  aksResourceName | The name of the Managed Cluster resource.  | pocAKSCluster    |
|  nodeResourceGroup | The name of AKS node resource group.  | pocNodeRG    |
|  vnetName | Name of the vnet the AKS Nodes will live  | pocVnet    |  
|  subnetName | Nameof the subnet the AKS Nodes will live  | AKS-SN    | 
|  dnsPrefix | Optional DNS prefix to use with hosted Kubernetes API server FQDN.  |     | 
|  vmSize | Size of the nodes to be deployed  | Standard_DS2_v2    | 
|  osDiskSizeGB | Size of the OS disk. Allowed values between 0-1023  | 1023    | 
|  kubernetesVersion | The version of Kubernetes.  | 1.7.7    | 
|  networkPlugin | Network plugin used for building Kubernetes network. Allowed values: azure or kubenet  | azure    |  
|  numNodes | Number of nodes to run in the cluster  | 3    |  
|  enableRBAC | Boolean flag to turn on and off of RBAC.  | true    | 
|  enablePrivateCluster | Enable private network access to the Kubernetes cluster.  | true    | 
|  enableHttpApplicationRouting | Boolean flag to turn on and off http application routing.  | false    |  
|  networkPolicy | Network policy used for building Kubernetes network.  | calico    | 
|  vnetSubnetID | Resource ID of the subnet where the nodes will exists  | [reference('deployVNET').outputs.aksSubnetID.value]    |  
|  serviceCidr | A CIDR notation IP range from which to assign service cluster IPs.  | 10.0.0.0/24    |
|  dnsServiceIP | Containers DNS server IP address.  | 10.0.0.10    |
|  dockerBridgeCidr | A CIDR notation IP for Docker bridge.  | 172.17.0.1/24    |

### Output  
"controlPlaneFQDN": The FQDN for the AKS Control Plane  
"aksID": The resource id for the AKS Cluster  

### Sample Deployment  

    {
      "name": "deployPrivateAKSCluster",
      "comments":"apiVersion is flagged, but haven't changed as not sure if this is needed for some features.",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2019-10-01",
      "dependsOn": [
        "deployVNET"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployAzureAKSTemplateURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "AksresourceName": {
            "value": "[variables('AksresourceName')]"
          },
          "location": {
              "value": "[resourceGroup().location]"
          },
          "nodeResourceGroup":{
            "value": "[variables('nodeResourceGroup')]"
          },
          "VNetName" : {
            "value": "[variables('vNETName')]"
          },
          "SubnetName" : {
            "value": "AKS-SN"
          },  
          "dnsPrefix": {
              "value": "[variables('dnsPrefix')]"
          },
          "kubernetesVersion": {
              "value": "[parameters('kubernetesVersion')]"
          },
          "networkPlugin": {
              "value": "[parameters('networkPlugin')]"
          },
          "enableRBAC": {
              "value": "[parameters('enableRBAC')]"
          },
          "vmssNodePool": {
              "value": "[parameters('vmssNodePool')]"
          },
          "windowsProfile": {
              "value": "[parameters('windowsProfile')]"
          },
          "enablePrivateCluster": {
              "value": "[parameters('enablePrivateCluster')]"
          },
          "enableHttpApplicationRouting": {
              "value": "[parameters('enableHttpApplicationRouting')]"
          },
          "networkPolicy": {
              "value": "[parameters('networkPolicy')]"
          },
          "vnetSubnetID": {
              "value": "[reference('deployVNET').outputs.aksSubnetID.value]"
          },
          "serviceCidr": {
              "value": "[variables('serviceCidr')]"
          },
          "dnsServiceIP": {
              "value": "[variables('dnsServiceIP')]"
          },
          "dockerBridgeCidr": {
              "value": "[variables('dockerBridgeCidr')]"
          },
          "resourceTag":{
            "value":"[variables('resourceTag')]"
          }
        }
      }
    }  
    
# <a name"ContainerTemplates"></a>Container Templates  

## <a name="AzureContainerRegistry"></a>Azure Container Registry Template  
This template will deploy an Azure Container Registry   

### Typical Nested Template used before  
NA     

### Typical Nested Template used after  
PrivateEndpoint  
PrivateDNSZone  
PrivateDNSARecord  
PrivateAKSMICluster          

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  acrName | Azure Container Registry name   | poc-acr    |  

### Output  
"acrId": The resource id of the Azure Container Registry created  

### Sample Deployment  

    {
      "name": "deployACR",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployACRURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "acrName": {
            "value": "[parameters('acrName')]"
          }
        }
      }
    }  

# <a name="Web Management"></a>Web Management  

## <a name="APIM"></a>APIM Template  
This template will deploy an Azure API Management Instance    

### Typical Nested Template used before  
NA    

### Typical Nested Template used after  
NA           

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  apimName | Name for the APIM instance  | poc-apim    |
|  sku | Allowed Values: Basic, Consumption, Developer, Standard, Premium  | Standard    |
|  capacity | Capacity of the SKU (number of deployed units of the SKU).  | 2   |  
|  apimEmail | Publisher email  | example@microsoft.com   |  
|  subnetID | Resource ID of the subnet that APIM will sit on | [concat(reference('deployVNET').outputs.vnetId.value,'/subnets/APIM-SN')]   |  
|  publisherName  | Publisher Name  | Microsoft  |
|  virtualNetworkType | Allowed Values: Internal, External | Internal |
|  disableGateway  | Boolean allowing you to diable gateway | false |

### Output  
"APIMIP": The resource id of the APIM Instance created  

### Sample Deployment  

    {
      "name": "deployAPIM",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployVNET",
        "deployAppInsights",
        "deployLAWorkspace"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployAPIMTemplateURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "apimname": {
            "value": "[parameters('apimName')]"
          },
          "sku": {
            "value": "[parameters('apimsku')]"
          },
          "capacity": {
            "value": "[parameters('apimcapacity')]"
          },
          "apimEmail": {
            "value": "[parameters('apimEmail')]"
          },
          "subnetID": {
            "value": "[concat(reference('deployVNET').outputs.vnetId.value,'/subnets/APIM-SN')]"
          },
          "publisherName": {
            "value": "[parameters('apimPublisherName')]"
          },
          "virtualNetworkType": {
            "value": "[parameters('apimVirtualNetworkType')]"
          },
          "disableGateway": {
            "value": "[parameters('apimDisableGateway')]"
          }
        }
      }
    }


## <a name="AzureRedisCacheVNet"></a>Azure Redis Cache VNet Integrated Template  
This template will deploy a premium version of Azure Redis Cache injected on a VNet  

### Typical Nested Templates used before
Vnet    

### Typical Nested Templates used after  
NA             

### Utilizing Template  
This template requires you to pass in the following parameters:  

| Parameter       | Description     | Example     |
| :------------- | :----------: | -----------: |
|  cahceName | Name for the Azure Redis Cache  | poc-azurecache    |
|  capacity | The size of the Redis cache to deploy  | 2    |
|  subnetId | Resource ID of the subnet it will reside on  | [concat(reference('deployVNET').outputs.vnetId.value,'/subnets/AzureBastionSubnet')]   |
|  saConnectionString  |  Storage Account Connection String  |  
|  ipAddress | IP Address to be assigned to cache  | 10.10.10.10   |  
|  backupEnabled | Boolean either enabling or disabling backup  | true   |
|  backupFrequency | How often to run a backup  | 90   |
|  maxSnapshots | Maximum number of snaphots allowed  | 10   |

### Output  
NA 

### Sample Deployment  

    {
      "name": "deployAzureCacheVault",
      "type": "Microsoft.Resources/deployments",
      "apiVersion": "2017-05-10",
      "dependsOn": [
        "deployLAWorkspace",
        "deployVNET"
      ],
      "properties": {
        "mode": "Incremental",
        "templateLink": {
          "uri": "[variables('deployAzureCacheBusURL')]",
          "contentVersion": "1.0.0.0"
        },
        "parameters": {
          "cacheName": {
            "value": "[parameters('cacheName')]"
          },
          "subnetId": {
            "value": "[concat(reference('deployVNET').outputs.vnetId.value,'/subnets/privateep-SN')]"
          },
          "saConnectionString": {
            "value": "[reference('deployPrivateStorage').outputs.saConnectionString.value]"
          },
          "ipAddress": {
            "value": "192.168.1.180"
          },
          "bakcupEnabled": {
            "value": true
          },
          "backupFrequency": {
            "value": 90
          },
          "maxSnaphots": {
            "value": 10
          }
        }
      }
    }  
# Nested Teamplates  
The Nested Templates are built to be generic in nature with the ability to pass in the values needed to build your specific architecture. Below you will find details on how to utilize each of the nested templates in this folder:  

# Templates  

[VNet](#VnetTemplate)  
[Empty NSG](#NSG-EMPTY-ExistingSubnetTemplate)  
[NSG with Rules](#NSG-ExistingSubnetTemplate)   
[Public IP Address](#PublicIPAddressTemplate)  
[Application Gateway with HTTP Listener](#AppGWHTTPListenerTemplate)

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
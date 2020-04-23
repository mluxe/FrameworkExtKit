
![Build](https://github.com/feilfly/FrameworkExtKit/workflows/Build/badge.svg?branch=master)

# Ldap Directory Service API
## Configurations
### .Net Core appsettings.json sample
```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "data source=(localdb)\\MSSQLLocalDB;initial catalog=FrameworkExtKit.NetCore.Tests;integrated security=true;multipleactiveresultsets=True"
      },
      "DirectoryAccountService": {
        "ClassName": "FrameworkExtKit.Services.DirectoryServices.LdapDirectoryAccountService"
        //"ClassName": "FrameworkExtKit.Services.DirectoryServices.ActiveDirectoryAccountService"
        //"ClassName": "FrameworkExtKit.Services.DirectoryServices.DB.DbDirectoryAccountService"
        //"ClassName": "FrameworkExtKit.UnitTest.Services.DirectoryService.MemoryDirectoryAccountService, FrameworkExtKit.UnitTest"
      },
      "LdapSettings": {
        "ServerName": "ldap.company.com",
        "ServerPort": 389,
        "UseSSL": false,
        "Credentials": {
          "DomainUserName": "",
          "Password": ""
        },
        "SearchBase": "o=company,c=an",
        "ContainerName": "",
        "DomainName": "company.com",
        "DomainDistinguishedName": "o=company,c=an"
      }
    }
```


### .Net Framework 4.5 App.Config/Web.Config
```xml
  <connectionStrings>
    <add name="DBDirectoryService" connectionString="data source=(localdb)\MSSQLLocalDB;integrated security=true;initial catalog=DBDirectoryService_Public;" providerName="System.Data.SqlClient" />
  </connectionStrings>
  <appSettings>
    <!-- User Directory Service-->
    <add key="DirectoryAccountService.ClassName" value="Framework.Services.DirectoryServices.LdapDirectoryAccountService" />
    <!--
    <add key="DirectoryAccountService.ClassName" value="Framework.Services.DirectoryServices.ActiveDirectoryAccountService" />
    <add key="DirectoryAccountService.ClassName" value="Framework.Services.DirectoryServices.DB.DbDirectoryAccountService" />
    <add key="DirectoryAccountService.ClassName" value="Framework.Services.DirectoryServices.MemoryDirectoryAccountService, FrameworkExtKit.UnitTest" />
    -->
    <add key="DirectoryAccountService.RootEntryPath" value="LDAP://ldap.company.com/o=company,c=an" />
    <!--<add key="DirectoryAccountService.RootEntryPath" value="DB://DBDirectoryService_Public" />-->
  </appSettings>
```


## How to use LdapQueryTranslator

```cs
// use namespace on .Net 4.5
// using System.DirectoryServices;
// using System.DirectoryServices.AccountManagement;

public class Server : DirectoryEntity {
    [DirectoryProperty("host")]
    public string HostName { get; set; }
    [DirectoryProperty("ip")]
    public string IP { get; set; }
    public string Location { get; set; }
}

class Sample {
    private LdapQueryTranslator GetTranslator(Expression<Func<Server, bool>> predicate) {
        return new LdapQueryTranslator(predicate, typeof(Server));
    }
    
    public Demo() {
        var translator = this.GetTranslator(s => s.HostName.Contains("server.com") || s.IP.StartWith("10.0." || s.Location == "Room 1");
        
        var filter = translator.FilterString;
        
        // output - (|(host=*server.com*)(ip=10.0.*)(Location=Room 1))
    }
}           
```

## How to use LdapDirectoryService and LdapDirectoryService<T>
    
```cs
using FrameworkExtKit.Services.DirectoryServices

///
/// LdapDirectoryService Sample
///

// default method
var service = new LdapDirectoryService();
// or with root Entry (.net 4.5)
DirectoryEntry rootEntry = new DirectoryEntry("ldap://ldap.company.com");
var service = new LdapDirectoryService(rootEntry)
// or with settings (.net core 2.1)
LdapSettings settings = new LdapSettings();
// remember to set setting properties
var service = new LdapDirectoryService(settings)

// search ldap
Server server = service.FindById<Server>("server-id");
server = service.Single<Server>(s => s.IP == "10.0.0.2");
IEnumerable<Server> servers = service.Where<Server>(s => s.IP.Contains("10.0.0.2"));


///
/// LdapDirectoryService<T> Sample
///

// default method
var service = new LdapDirectoryService<Server>();
// or with root Entry (.net 4.5)
DirectoryEntry rootEntry = new DirectoryEntry("ldap://ldap.company.com");
var service = new LdapDirectoryService<Server>(rootEntry)
// or with settings (.net core 2.1)
LdapSettings settings = new LdapSettings();
// remember to set setting properties
var service = new LdapDirectoryService<Server>(settings)

// search ldap
Server server = service.FindById("server-id");
server = service.FindByName("host-name");
server = service.Single(s => s.IP == "10.0.0.2");
IEnumerable<Server> servers = service.Where(s => s.IP.Contains("10.0.0.2"));

```


## How to use IDirectoryAccountService<T>, IDirectoryRoleAccountService<T>, IDirectoryGroupService<T> and IDirectoryDistributionListService<T>


### implement the entity class for each service you would like to use

```cs
class SampleAccount : DirectoryAccount {
    [DirectoryProperty("localcountry")]
    
    public string ResidentCountry { get; set; }
    //....  and other properties
}

class SampleGroup : DirectoryGroup {
    // add properties
}

class SampleRoleAccount : DirectoryRoleAccount {
    // add properties
}

class SampleDistributionList : DirectoryDistributionList {
    // add properties
}
```

### use the services to search ldap

```cs
IDirectoryAccountService<SampleAccount>         account_service = new LdapDirectoryAccountService<SampleAccount>();
IDirectoryAccountService<SampleGroup>           group_service = new LdapDirectoryAccountService<SampleGroup>();
IDirectoryAccountService<SampleRoleAccount>     role_account_service = new LdapDirectoryAccountService<SampleRoleAccount>();
IDirectoryAccountService<SampleDistributionList> distriution_list_service = new LdapDirectoryAccountService<SampleDistributionList>();

var account = account_service.FindById(1234);
var manager = account_service.FindDirectManager(account);

// similar usage for other services
```

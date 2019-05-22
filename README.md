Build status
[![Build Status](https://dev.azure.com/OpenRiaServices/OpenRiaServices/_apis/build/status/OpenRIAServices.OpenRiaServices.FluentMetadata?branchName=master)](https://dev.azure.com/OpenRiaServices/OpenRiaServices/_build/latest?definitionId=3&branchName=master)

Sonarcloud status
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OpenRIAServices_OpenRiaServices.FluentMetadata&metric=alert_status)](https://sonarcloud.io/dashboard?id=OpenRIAServices_OpenRiaServices.FluentMetadata)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=OpenRIAServices_OpenRiaServices.FluentMetadata&metric=security_rating)](https://sonarcloud.io/dashboard?id=OpenRIAServices_OpenRiaServices.FluentMetadata)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=OpenRIAServices_OpenRiaServices.FluentMetadata&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=OpenRIAServices_OpenRiaServices.FluentMetadata)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=OpenRIAServices_OpenRiaServices.FluentMetadata&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=OpenRIAServices_OpenRiaServices.FluentMetadata)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=OpenRIAServices_OpenRiaServices.FluentMetadata&metric=bugs)](https://sonarcloud.io/dashboard?id=OpenRIAServices_OpenRiaServices.FluentMetadata)

Nuget [![Nuget](https://img.shields.io/nuget/v/OpenRiaServices.FluentMetadata.svg)](https://www.nuget.org/packages/OpenRiaServices.FluentMetadata)
preview [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/OpenRiaServices.FluentMetadata.svg)](https://www.nuget.org/packages/OpenRiaServices.FluentMetadata)

# OpenRiaServices.FluentMetadata

FluentMentadata is a Fluent API for defining metadata for Open RIA Services entities

With permission of Nikhil Kothari, I've added the source code of Nikhil's Fluent API for defining WCF Ria Services Metadata to RIA Services Contrib. 
I've made a couple of changes. Most noticeable is the ability to attach a MetadataClass to your domain service using the FluentMetadata attribute. This class mimics the OnModelCreating method in EntityFramework DbContext and allows you to instantiate your Metadata classes. 
The main advantage of this package is that it allows you to separate your datamodel and metadata in separate projects/assemblies. This is not possible with the standard RIA services approach of buddy classes. These have to be defined in the same project as your datamodel. The drawbacks of this are a pollution of your datamodel and (most importantly) that your datamodel project becomes dependent on RIA services-specific assemblies. With the FluentMetadata package you can define metadata in any assembly you like.

## Download
The package is available as a [Nuget package](https://www.nuget.org/packages/OpenRiaServices.FluentMetadata).


Last nuget release [![Nuget](https://img.shields.io/nuget/v/OpenRiaServices.FluentMetadata.svg)](https://www.nuget.org/packages/OpenRiaServices.FluentMetadata)

Last preview release [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/OpenRiaServices.FluentMetadata.svg)](https://www.nuget.org/packages/OpenRiaServices.FluentMetadata)


## Using FluentMetadata
* Install the FluentMetadata Nuget package (or download its source code)
* Define a MetadataConfiguration class containing the fluent metadata for your entities. For example:

```csharp
public class FluentMetadataConfiguration : IFluentMetadataConfiguration
{
    public void OnTypeCreation(MetadataContainer metadataContainer)
    {
        metadataContainer.Entity<Foo>().Projection(x => x.SomeProperty).Exclude();
    }
}
```

* Alternatively, you can define the metadata for each entity in separate MetadataClass classes. For example
```csharp
public class FooMetadata : MetadataClass<Foo>
{
    public FooMetadata()
    {
        this.Projection(x => x.ExcludedString).Exclude();
        this.Validation(x => x.RequiredString).Required();
        this.Validation(x => x.RegularExpressionString).RegularExpression("[a-z](a-z)");
    }
}
```
* ... and instantiate it from your FluentMetadataConfiguration  class, like so:
```csharp
public class FluentMetadataConfiguration : IFluentMetadataConfiguration
{
    public void OnTypeCreation(MetadataContainer metadataContainer)
    {
        metadataContainer.Add(new FooMetadata());
    }
}
```
* ... both mechanisms can be combined.
* Lastly, add the FluentMetadata attribute to your domain service:

```csharp
[EnableClientAccess()]
[FluentMetadata(typeof(FluentMetadataConfiguration))]
public class FluentMetadataTestDomainService : DomainService
{
}
```

## Sample project

Look into the FluentMetadata.Tests folder

## Previous versions
* The version for WCF Ria Services can be found in [riaservicescontrib
](https://github.com/OpenRIAServices/riaservicescontrib)
* The original version is available [here](http://www.nikhilk.net/RIA-Services-Fluent-Metadata-API.aspx).

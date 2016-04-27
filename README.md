# Observable Entities Light Tracking and Validation
Observable Entities Light Tracking and Validation is a light framework that provides the following functionalities:

<ul>
<li>Entities change tracking</li>
<li>Support for automatic validation on a property change</li>
<li>Support for automatic notification on validation errors</li>
<li>Support for validation with custom error severity levels</li>
<li>Validation for duplicates on composite primary keys</li>
<li>Complex custom validation through a pluggable <code>IServiceProvider</code></li>
</ul>
<p>You can add the nuget package to your project: https://www.nuget.org/packages/ObservableEntitiesLightTracking/.</p>
<p>I intend to post shortly an additional usage guide and add several samples: a sample for working with POCO entities, a sample on using this in the MVVM world, a sample to emphasize the benefit of using this to integrate with a Web API (create, update, partial updates, delete) and a sample to emphasize using this on the server side when your Web API is in front of a more complex server side architecture that doesn't make it so straight forward to use Entity Framework.</p>
### <a name="func1"></a>1. Entities change tracking
The entities could be simple POCO classes or classes with richer client behaviour like implementing `INotifyPropertyChanged` or `INotifyDataErrorInfo`. Entities are added to a context and managed by a different Entity Set for each entity type (pretty similar to the Entity Framework DBContext and DBSet concepts). Entities could be attached, detached, added, deleted from their particular entity set. While an entity is added/attached to a context, it will be tracked and all it's changes could be retrieved from the context until the moment the changes are applied, cancelled or the entity is detached from the context.
Ignoring changes on certain properties could be achieved by decorating them with the `IgnoreProperty` attribute.

### <a name="func2"></a>2. Automatic validation on a property change
Entities implementing `INotifyPropertyChanged` could be leveraged to perform automatic validation on the property change event, just by one line of code against the entity set:<br/>
`context.Set<MyEntityType>().ValidateOnPropertyChanged = true;`<br/>

### <a name="func3"></a>3. Automatic notification on validation errors
In order to use this functionality, the entity being validated should implement the `IWriteDataErrorInfo` interface, defined in `ObservableEntitiesLightTracking.ComponentModel`. The entity can be notified this way of any validation errors and process them (e.g. for display purposes).

### <a name="func4"></a>4. Validation with custom error severity levels
Custom client validation with customizable severity levels is supported. Entities must implement interface `IValidatableObjectWithSeverityLevel` for this to happen. It could be possible that sometimes it would be required that some of the error severity levels would not make the validation fail (while still being necessary for display purposes). This could be achieved by one line of code:<br/>
`context.Configuration.ValidationSafeSeverityLevels = new Collection<object>() { ValidationSeverityLevel.Warning};`<br/>
You could have other severity levels like `ValidationSeverityLevel.CriticalError, ValidationSeverityLevel.Error` which would make the validation fail because they are not added to the collection above.

### <a name="func5"></a>5. Validation for duplicates on composite primary keys
The `KeyProperty` validation attribute could be used to mark one or more properties of the entities as primary key of the entity context. Having duplicates would the be picked by the validation and if the entity is implementing the `IWriteDataErrorInfo` interface, it would then be notified of this error.

### <a name="func6"></a>6. Complex custom validation through a pluggable `IServiceProvider`
The `OEContext` could be configured with a custom `IServiceProvider` by one line of code:<br/>
`context.Configuration.ValidationServiceProvider = myCustomProvider`<br/>
Such a provider could be a dependency injection container. This way when you implement the validation on your entities through `IValidatableObject` or `IValidatableObjectWithSeverityLevel` you could take advantage of your custom service provider that is passed to the validation methods through the `DataContext`. You could then write more complex validation (e.g. going back to your data repositories or your web servers to fetch more data).

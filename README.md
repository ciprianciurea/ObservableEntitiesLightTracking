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

### <a name="func1"></a>1. Entities change tracking
The entities could be simple POCO classes or classes with richer client behaviour like implementing <code>INotifyPropertyChanged</code> or <code>INotifyDataErrorInfo</code>. Entities are added to a context and managed by a different Entity Set for each entity type (pretty similar to the Entity Framework DBContext and DBSet concepts). Entities could be attached, detached, added, deleted from their particular entity set. While an entity is added/attached to a context, it will be tracked and all it's changes could be retreived from the context untill the moment the changes are applied, cancelled or the entity is detached from the context.

### <a name="func2"></a>2. Automatic validation on a property change
Entities implementing <code>INotifyPropertyChanged</code> could be leveraged to perform automatic validation on the property change event, just by one line of code against the entity set:<br/>
<code>context.Set<MyEntityType>().ValidateOnPropertyChanged = true;</code><br/>

### <a name="func3"></a>3. Automatic notification on validation errors
In order to use this functionality, the entity being validated should implement the <code>IWriteDataErrorInfo</code> interface, defined in <code>ObservableEntitiesLightTracking.ComponentModel</code>. The entity can be notified this way of any validation errors and process them (e.g. for display purposes).

### <a name="func4"></a>4. Validation with custom error severity levels
Custom client validation with customizable severity levels is supported. Entities must implement interface <code>IValidatableObjectWithSeverityLevel</code> for this to happen. It could be possible that sometimes it would be required that only some of the error severity levels would make the validation fail (the others still being necessary for display purposes). This could be achieved by one line of code:<br/>
<code>context.Configuration.ValidationSafeSeverityLevels = new Collection<object>() { ValidationSeverityLevel.CriticalError, ValidationSeverityLevel.Error };</code><br/>
You could have other severity levels like <code>ValidationSeverityLevel.Warning</code> which would still be picked up by the validation, but would not fail it.

### <a name="func5"></a>5. Validation for duplicates on composite primary keys
The <code>KeyProperty</code> validation attribute could be used to mark one or more properties of the entities as primary key of the entity context. Having duplicates would the be picked by the validation and if the entity is implementing the <code>IWriteDataErrorInfo</code> interface, it would then be notified of this error.

### <a name="func6"></a>6. Complex custom validation through a pluggable <code>IServiceProvider</code>
The <code>OEContext</code> could be configured with a custom <code>IServiceProvider</code> by one line of code:<br/>
<code>context.Configuration.ValidationServiceProvider = myCustomProvider</code><br/>
Such a provider could be a dependency injection container. This way when you implement the validation on your entities through <code>IValidatableObject</code> or <code>IValidatableObjectWithSeverityLevel</code> you could take advantage of your custom service provider that is passed to the validation methods through the <code>DataContext</code>. You could then write more complex validation (e.g. going back to your data repositories or your web servers to fetch more data).

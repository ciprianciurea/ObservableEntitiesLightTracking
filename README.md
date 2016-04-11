# Observable Entities Light Tracking and Validation
Observable Entities Light Tracking and Validation is a light framework that provides the following functionalities:

<ul>
<li>[Entities change tracking](#func1)</li>
<li>Support for automatic validation on a property change</li>
<li>Support for automatic notification on validation errors</li>
<li>Support for validation with custom client severity levels</li>
<li>Validation for duplicates on composite primary keys</li>
<li>Complex custom validation through a pluggable <code>IServiceProvider</code></li>
</ul>

### <a name="func1"></a>1. Entities change tracking
The entities could be simple POCO classes or classes with richer client behaviour like implementing <code>INotifyPropertyChanged</code> or <code>INotifyDataErrorInfo</code>. Entities are added to a context and managed by a different Entity Set for each entity type (pretty similar to the Entity Framework DBContext and DBSet concepts). Entities could be attached, detached, added, deleted from their particular entity set. While an entity is added/attached to a context, it will be tracked and all it's changes could be retreived from the context untill the moment the changes are applied, cancelled or the entity is detached from the context.

# Observable Entities Light Tracking and Validation
Observable Entities Light Tracking and Validation is a light framework that provides the following functionalities:

## 1) Entities change tracking
The entities could be simple POCO classes or classes with richer client behaviour like implementing <code>INotifyPropertyChanged</code> or <code>INotifyDataErrorInfo</code>. Entities are added to a context and managed by a different Entity Set for each entity type (pretty similar to the Entity Framework DBContext and DBSet concepts). Entities could be attached, detached, added, deleted from their particular entity set. While an entity is added/attached to a context, it will be tracked and all it's changes could be retreived from the context untill the moment the changes are applied, cancelled or the entity is detached from the context.

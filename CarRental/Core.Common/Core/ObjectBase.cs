using Core.Common.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using FluentValidation;
using FluentValidation.Results;
using System.Text;
using System.Linq.Expressions;
using Core.Common.Utils;

namespace Core.Common.Core
{
    [DataContract]
    public abstract class ObjectBase : NotificationObject, IExtensibleDataObject, IDirtyCapable, IDataErrorInfo
    {
        public ObjectBase()
        {
            _Validator = GetValidator();
            Validate();
        }
     
        protected bool _IsDirty = false;
        protected IValidator _Validator = null;
        private IList<ValidationFailure> _ValidationErrors;

        #region IDirtyCapable members

        [NotNavigable]
        public virtual bool IsDirty
        {
            get { return _IsDirty; }
            protected set
            {
                _IsDirty = value;
                OnPropertyChanged("IsDirty", false);
            }
        }

        public virtual bool IsAnythingDirty()
        {
            bool isDirty = false;

            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                {
                    isDirty = true;
                    return true;
                }
                else
                    return false;

            }, coll => { });

            return isDirty;
        }

        public List<IDirtyCapable> GetDirtyObjects()
        {
            List<IDirtyCapable> dirtyObjects = new List<IDirtyCapable>();

            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                    dirtyObjects.Add(o);

                return false;
            }, coll => { });

            return dirtyObjects;
        }


        public void CleanAll()
        {
            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                    o.IsDirty = false;
                return false;
            }, coll => { });
        }

        #endregion


        protected void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            base.OnPropertyChanged(propertyName);

            if (makeDirty)
                IsDirty = true;

            Validate();
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName, makeDirty);
        }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion

        #region protected

        protected void WalkObjectGraph(Func<ObjectBase,bool> snippetForObject,
                                       Action<IList> snippetForCollection,
                                       params string [] exemptProperties)
        {
            List<ObjectBase> visited = new List<ObjectBase>();
            Action<ObjectBase> walk = null;

            List<string> exemptions = new List<string>();
            if (exemptProperties != null)
                exemptions = exemptProperties.ToList();

            walk = (o) =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);

                    bool exitWalk = snippetForObject.Invoke(o);

                    if(!exitWalk)
                    {
                        PropertyInfo[] properties = o.GetBrowsableProperties();
                        foreach(PropertyInfo property in properties)
                        {
                            if(!exemptions.Contains(property.Name))
                            {
                                if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                                {
                                    ObjectBase obj = (ObjectBase)(property.GetValue(o, null));
                                    walk(obj);
                                }
                                else
                                {
                                    IList coll = property.GetValue(o, null) as IList;
                                    if(coll != null)
                                    {
                                        snippetForCollection.Invoke(coll);

                                        foreach(object item in coll)
                                        {
                                            if (item is ObjectBase)
                                                walk((ObjectBase)item);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            };

            walk(this);
        }

        #endregion

        #region Validation

        protected virtual IValidator GetValidator()
        {
            return null;
        }

        public IEnumerable<ValidationFailure> ValidationErrors
        {
            get { return _ValidationErrors; }
            set { }
        }

        public void Validate()
        {
            if(_Validator != null)
            {
                ValidationResult results = _Validator.Validate(this);
                _ValidationErrors = results.Errors;
            }
        }

        [NotNavigable]
        public virtual bool IsValid
        {
            get
            {
                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                    return false;
                else
                    return true;
            }
        }

        #endregion

        #region IDataErrorInfo members


        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                StringBuilder errors = new StringBuilder();

                if(_ValidationErrors != null && _ValidationErrors.Count() > 0)
                {
                    foreach(ValidationFailure validationError in _ValidationErrors)
                    {
                        if (validationError.PropertyName == columnName)
                            errors.AppendLine(validationError.ErrorMessage);
                    }
                }

                return errors.ToString();
            }
        }

        public string this[string columnName] => throw new NotImplementedException();

        #endregion


        private PropertyInfo[] GetBrowsableProperties()
        {
            throw new NotImplementedException();
        }
    }
}

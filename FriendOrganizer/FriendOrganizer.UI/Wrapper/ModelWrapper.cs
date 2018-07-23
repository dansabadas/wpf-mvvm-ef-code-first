﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FriendOrganizer.UI.Wrapper
{
    public abstract class ModelWrapper<T> : NotifyDataErrorInfoBase
    {
        protected ModelWrapper(T model)
        {
            Model = model;
        }

        public T Model { get; }

        protected void SetValue<TValue>(TValue value, [CallerMemberName]string propertyName = "")
        {
            typeof(T).GetProperty(propertyName)?.SetValue(Model, value);
            OnPropertyChanged(propertyName);
            ValidatePropertyInternal(propertyName, value);
        }

        protected TValue GetValue<TValue>([CallerMemberName]string propertyName = "")
        {
            return (TValue)typeof(T).GetProperty(propertyName)?.GetValue(Model);
        }

        private void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);
      
            ValidateDataAnnotations(propertyName, currentValue);
      
            ValidateCustomErrors(propertyName);
        }

        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(Model) { MemberName = propertyName };
            Validator.TryValidateProperty(currentValue, context, results);

            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        protected abstract IEnumerable<string> ValidateProperty(string propertyName);
    }
}

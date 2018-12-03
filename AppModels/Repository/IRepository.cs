using System;
using System.Collections;
using System.Collections.Generic;

namespace AppModels.Repository {
    public interface IRepository<T> {
        IEnumerable<T> GetByGroupId(int groupId);
        IEnumerable<T> GetAll();
        void Add(T item);
        void Update();
        void Delete(T item);
    }
}
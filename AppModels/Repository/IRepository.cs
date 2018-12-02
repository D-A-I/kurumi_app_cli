using System;
using System.Collections;
using System.Collections.Generic;

namespace AppModels.Repository {
    public interface IRepository<T> {
        T GetOne (int id);
        IEnumerable<T> GetAll ();
        void Add (T item);
        void Update (T item);
        void Delete (T item);
    }
}
using System.Collections.Generic;
using LSB.Interfaces;

namespace LSB.Classes.ObjectPool {
	public class ObjectPool : IPool {
		private readonly List<IPooledObject> _objects;
		
		private readonly IPooledObject _pooledObject;
		private readonly bool _allowAddNew;

		private int _activeObjects;
		
		public ObjectPool(IPooledObject pooledObject, bool allowAddNew) {
			_pooledObject = pooledObject;
			_allowAddNew = allowAddNew;
			_objects = new List<IPooledObject>();
			_activeObjects = 0;
		}
		
		public IPooledObject Get() {
			int i = -1;
			bool searching = true;

			while (i < _objects.Count && searching) {
				i++;
				
				if (!_objects[i].Active) {
					_objects[i].Active = true;
					_activeObjects++;
					searching = false;
				}
			}

			if (!searching) return _objects[i];

			if (!_allowAddNew) return null;

			IPooledObject newObject = createNewObject();
			newObject.Active = true;
			_objects.Add(newObject);
			_activeObjects++;

			return newObject;
		}

		public void Release(IPooledObject obj) {
			obj.Active = false;
			_activeObjects--;
			obj.Reset();
		}

		private IPooledObject createNewObject() {
			IPooledObject newObject = _pooledObject.Clone();
			return newObject;
		}

		public int GetCount() {
			return _objects.Count;
		}

		public int GetActive() {
			return _activeObjects;
		}
	}
}

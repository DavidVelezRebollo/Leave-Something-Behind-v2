namespace LSB.Interfaces {
	public interface IPool {
		public IPooledObject Get();
		public void Release(IPooledObject obj);
	}
	
}

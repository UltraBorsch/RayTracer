using UnityEngine;

public interface IStructable<T> where T : struct {
    public T GetStruct();
}

using UnityEngine;

public interface IStructable<T> where T : struct {
    public T Struct { get; }

    public void SetupStruct();
}

using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{
    [SerializeField] PlayerInputs _inputs;
    public override void InstallBindings()
    {
        Container.BindInstance<PlayerInputs>(_inputs).AsSingle();
    }
}
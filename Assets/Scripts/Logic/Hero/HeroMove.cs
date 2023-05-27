using System;
using Services.Input;
using UnityEngine;

namespace Logic.Hero
{
  public class HeroMove : MonoBehaviour
  {
    [SerializeField] private CharacterController characterController;
    [SerializeField] private HeroAttack heroAttack;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private IInputService _inputService;
    private Camera _camera;
    private Vector3 _movementVector;
    private Transform _target;

    public void Construct(IInputService inputService) => _inputService = inputService;

    private void Start() =>
      _camera = Camera.main;

    private void Update()
    {
      _movementVector = Vector3.zero;
      
      if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
      {
        _movementVector = _camera.transform.TransformDirection(_inputService.Axis);
        _movementVector.y = 0;
        _movementVector.Normalize();

        transform.forward = _movementVector;
      }

      _movementVector += Physics.gravity;
      
      characterController.Move(movementSpeed * _movementVector * Time.deltaTime);
      
      RotateToTarget();
    }

    private void RotateToTarget()
    {
      _target = heroAttack.target;
      if (_target != null)
      {
        var targetDirection = (_target.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.Euler(0, Quaternion.LookRotation(targetDirection).eulerAngles.y, 0);
        
        transform.rotation = Quaternion.RotateTowards(
          transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
      }
    }

    /*public void UpdateProgress(PlayerProgress progress)
    {
      progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
    }

    public void LoadProgress(PlayerProgress progress)
    {
      if (CurrentLevel() != progress.WorldData.PositionOnLevel.Level) return;

      Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
      if (savedPosition != null) 
        Warp(to: savedPosition);
    }

    private static string CurrentLevel() => 
      SceneManager.GetActiveScene().name;

    private void Warp(Vector3Data to)
    {
      _characterController.enabled = false;
      transform.position = to.AsUnityVector().AddY(_characterController.height);
      _characterController.enabled = true;
    }*/
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Key
{
    Up = 0,
    Down,
    Left,
    Right,
    Jump,
    Aim,
    Shoot,
    EXShoot,
    Switch,
    Dash
};


[CreateAssetMenu]
public class Key_Data : ScriptableObject
{
    [SerializeField] private KeyCode _UpKey;
    [SerializeField] private KeyCode _DownKey;
    [SerializeField] private KeyCode _LeftKey;
    [SerializeField] private KeyCode _RightKey;
    [SerializeField] private KeyCode _JumpKey;
    [SerializeField] private KeyCode _AimKey;
    [SerializeField] private KeyCode _ShootKey;
    [SerializeField] private KeyCode _EXShootKey;
    [SerializeField] private KeyCode _SwitchWeaponKey;
    [SerializeField] private KeyCode _DashKey;

    

    public KeyCode DownKey =>_DownKey;
    public KeyCode UpKey => _UpKey;
    public KeyCode LeftKey => _LeftKey;
    public KeyCode RightKey => _RightKey;
    public KeyCode JumpKey => _JumpKey;
    public KeyCode AimKey => _AimKey;
    public KeyCode ShootKey => _ShootKey;
    public KeyCode EXShootKey => _EXShootKey;
    public KeyCode SwitchWeaponKey => _SwitchWeaponKey;
    public KeyCode DashKey => _DashKey;
    public void ChangeKey(KeyCode keyCode, int ChangeKeyNum)
    {
        switch ((Key)ChangeKeyNum)
        {
            case Key.Up:
                _UpKey = keyCode;
                break;
            case Key.Down:
                _DownKey = keyCode;
                break;
            case Key.Left:
                _LeftKey = keyCode;
                break;
            case Key.Right:
                _RightKey = keyCode;
                break;
            case Key.Jump:
                _JumpKey = keyCode;
                break;
            case Key.Aim:
                _AimKey = keyCode;
                break;
            case Key.Shoot:
                _ShootKey = keyCode;
                break;
            case Key.EXShoot:
                _EXShootKey = keyCode;
                break;
            case Key.Switch:
                _SwitchWeaponKey = keyCode;
                break;
            case Key.Dash:
                _DashKey = keyCode;
                break;
        }

    }

    public void DefaultKey()
    {

        _UpKey = KeyCode.UpArrow;
        _DownKey = KeyCode.DownArrow;
        _LeftKey = KeyCode.LeftArrow;
        _RightKey = KeyCode.RightArrow;
        _JumpKey = KeyCode.C;
        _AimKey = KeyCode.X;
        _ShootKey = KeyCode.Z;
        _EXShootKey = KeyCode.A;
        _SwitchWeaponKey = KeyCode.LeftShift;
        _DashKey = KeyCode.Space;
                
    }

    public KeyCode GetKey(Key CheckKey)
    {
        KeyCode keyCode = default;
        switch (CheckKey)
        {
            case Key.Up:
                keyCode = _UpKey;
                break;
            case Key.Down:
                keyCode = _DownKey;
                break;
            case Key.Left:
                keyCode = _LeftKey;
                break;
            case Key.Right:
                keyCode = _RightKey;
                break;
            case Key.Jump:
                keyCode = _JumpKey;
                break;
            case Key.Aim:
                keyCode = _AimKey;
                break;
            case Key.Shoot:
                keyCode = _ShootKey;
                break;
            case Key.EXShoot:
                keyCode = _EXShootKey;
                break;
            case Key.Switch:
                keyCode = _SwitchWeaponKey;
                break;
            case Key.Dash:
                keyCode = _DashKey;
                break;
        }
        return keyCode;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hand
{
    Left,
    Right
}
public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public Hand handSlot;
    public GameObject currentWeaponModel;

    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        if (weaponItem == null)
            return;
        UnloadWeapon();
        GameObject model = Instantiate(weaponItem.model);
        if (model != null)
        {
            if (parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;           
        }
        currentWeaponModel = model;
    }
    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }
    public void LoadWeapon()
    {
        if (currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(true);
        }
    }
    public void DestroyWeapon()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }
}

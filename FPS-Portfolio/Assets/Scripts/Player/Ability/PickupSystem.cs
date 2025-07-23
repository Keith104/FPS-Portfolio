using UnityEngine;

public class PickupSystem : MonoBehaviour, IPickup
{

    public bool hasGrenadeBullet;

    public float dotTimer;
    public float damageRate;
    public float damageAmount;

    public void GetPickupItem(PickupItems item)
    {
        if (item.abilitiesType == PickupItems.abilities.none)
        {
            GameManager.instance.playerController.TakeDamage(-item.healthInc, Damage.damagetype.stationary);
            GameManager.instance.playerController.swappingSystem.primary.currentHeldAmmo += item.primaryAmmoInc;
            GameManager.instance.playerController.swappingSystem.secondary.currentHeldAmmo += item.secondaryAmmoInc;
            GameManager.instance.playerController.swappingSystem.nonLethalSpawner.currentHeldAmmo += item.nonLethalAmmoInc;
            GameManager.instance.playerController.swappingSystem.lethalSpawner.currentHeldAmmo += item.lethalAmmoInc;

            if (GameManager.instance.playerController.swappingSystem.primary.enabled == true)
            {
                GameManager.instance.playerController.swappingSystem.primary.updateGunUI();
            }
            else if (GameManager.instance.playerController.swappingSystem.secondary.enabled == true)
            {
                GameManager.instance.playerController.swappingSystem.secondary.updateGunUI();
            }
            else if (GameManager.instance.playerController.swappingSystem.nonLethalSpawner.transform.childCount > 0)
            {
                GameManager.instance.playerController.swappingSystem.nonLethalSpawner.updateThrowableUI();
            }
            else if (GameManager.instance.playerController.swappingSystem.lethalSpawner.transform.childCount > 0)
            {
                GameManager.instance.playerController.swappingSystem.lethalSpawner.updateThrowableUI();
            }
        }
        else
        {
            WeaponSelection currWeapon = GetComponentInParent<PlayerController>().Gun;

            switch (item.abilitiesType)
            {
                case PickupItems.abilities.grenadeBullets:
                    hasGrenadeBullet = true;
                    currWeapon.SetBullet(item.bulletModel);
                    break;
                case PickupItems.abilities.homingBullets:
                    currWeapon.SetBullet(item.bulletModel);
                    Debug.Log("Setting Bullet");
                    break;
                case PickupItems.abilities.fireDamage:
                    currWeapon.SetBullet(item.bulletModel);
                    damageAmount = 0.4f;
                    damageRate = 1;
                    dotTimer = 2;
                    break;
                case PickupItems.abilities.poisonDamage:
                    currWeapon.SetBullet(item.bulletModel);
                    damageAmount = 0.2f;
                    damageRate = 0.4f;
                    dotTimer = 3;
                    break;
            }
        }

    }
}

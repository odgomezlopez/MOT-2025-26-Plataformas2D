using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class StatsComponent : MonoBehaviour, IDamageable 
{
    public Stats stats;

    private void Awake()
    {
        //Inicializaciones de los stats
        stats.invulnerability = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }
    private void OnDisable()
    {
        stats.invulnerability = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }

    public void TakeDamage(float damage, GameObject org)
    {
        //Si el personaje es invulnerable, no se le hace daño
        if (stats.invulnerability) return;

        //Resto el daño a la vida actual
        stats.HP.Value -= damage;

        if(stats.HP.Value != 0) TemporalInvulnerability(stats.invulnerabilityDuration);
    }

    public void InstaKill()
    {
        stats.HP.Value = 0;
    }

    public void TemporalInvulnerability(float invTime,bool ignoreColision=true,bool changeColor=true)
    {
        if (stats.invulnerability == true) return;

        stats.invulnerabilityChangeColor = changeColor;
        StartCoroutine(HandleInvulnerability(invTime, ignoreColision));
    }
    private IEnumerator HandleInvulnerability(float invTime,bool ignoreColision = true)
    {
        stats.invulnerability = true;
        if(ignoreColision) Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        //Se evita las colisiones con los objetos en la capa "Enemy" para que el jugador no reciba daño mientras es invulnerable y que pueda traspasarlos
        //NO ponerlo NUNCA la layer "Enemy" a la zonas de caida, ya que el jugador podría caer sin poder morir

        yield return new WaitForSeconds(invTime);

        stats.invulnerability = false;
        if (ignoreColision) Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }

    private void OnValidate()
    {
        stats.HP.ForceNotify();
    }
}

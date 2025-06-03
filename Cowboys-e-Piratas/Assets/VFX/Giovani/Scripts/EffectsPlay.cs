using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EffectsPlay : MonoBehaviour
{
    public GameObject currentEffect;
    GameObject[] effectsList;
    int index = 0;

    void Awake()
    {
        var list = new List<GameObject>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            var effect = this.transform.GetChild(i).gameObject;
            list.Add(effect);
        }
        effectsList = list.ToArray();

        PlayAtIndex();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentEffect != null)
            {
                var ps = currentEffect.GetComponent<ParticleSystem>();
                if (ps.isEmitting)
                {
                    ps.Stop(true);
                }
                else
                {
                    if (!currentEffect.gameObject.activeSelf)
                    {
                        currentEffect.SetActive(true);
                    }
                    else
                    {
                        ps.Play(true);
                    }
                }
            }
        }

        /* if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
        {
            if (currentEffect != null)
            {
                currentEffect.SetActive(false);
                currentEffect.SetActive(true);
            }
        } */

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousEffect();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextEffect();
        }
    }

    public void NextEffect()
    {
        index++;
        WrapIndex();
        PlayAtIndex();
    }

    public void PreviousEffect()
    {
        index--;
        WrapIndex();
        PlayAtIndex();
    }
    
    void WrapIndex()
		{
			if (index < 0) index = effectsList.Length - 1;
			if (index >= effectsList.Length) index = 0;
		}
        
    public void PlayAtIndex()
    {
        if (currentEffect != null)
        {
            currentEffect.SetActive(false);
        }

        currentEffect = effectsList[index];
        currentEffect.SetActive(true);
    }
}

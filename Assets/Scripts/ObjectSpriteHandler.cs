using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpriteHandler : MonoBehaviour
{
    [SerializeField]
    //SpriteRenderer objectSpriteRenderer;
    List<SpriteRenderer> objectSpriteRenderer;
    [SerializeField]
    Animator objectAnimator;

    //Color objectOriginalColor;
    List<Color> objectOriginalColor = new List<Color>();

    Color currentColor;

    List<Coroutine> colorCoroutines = new List<Coroutine>();

    private void Start()
    {
        this.objectSpriteRenderer = new List<SpriteRenderer>();
        this.objectOriginalColor = new List<Color>();
        /*if (this.GetComponent<SpriteRenderer>())
        {
            this.objectSpriteRenderer.Add(this.GetComponent<SpriteRenderer>());
        }*/
        foreach (SpriteRenderer sRenderer in this.GetComponentsInChildren<SpriteRenderer>())
        {
            this.objectSpriteRenderer.Add(sRenderer);
        }
        /*
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (this.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>())
            {
                this.objectSpriteRenderer.Add(this.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>());
            }
        }*/
        foreach (SpriteRenderer sRenderer in this.objectSpriteRenderer)
        {
            this.objectOriginalColor.Add(sRenderer.color);
        }
        if (!this.objectAnimator)
        {
            if (!(this.objectAnimator = this.GetComponent<Animator>()))
            {
                if (!(this.objectAnimator = this.GetComponentInChildren<Animator>()))
                {
                    Debug.Log("No se ha encontrado el animator del objeto " + this.gameObject.name);
                }
            }
        }
    }



    public void TemporarilyChangeColor(Color color){
        if (this.currentColor != color)
        {
            if (color == Color.white)
            {
                ResetToOriginalColor();
            }
            else
            {
                this.currentColor = color;
                for (int i = 0; i < this.objectSpriteRenderer.Count; i++)
                {
                    this.objectSpriteRenderer[i].color = color;
                }
            }
        }
    }


    public void ResetToOriginalColor()
    {
        if (this.currentColor != Color.white)
        {
            for (int i = 0; i < this.objectSpriteRenderer.Count; i++)
            {
                this.objectSpriteRenderer[i].color = this.objectOriginalColor[i];
            }
            this.currentColor = Color.white;
        }
    }

    public void ChangeColorOverTime(float time, Color color)
    {
        if (this.colorCoroutines.Count > 0)
        {
            foreach (Coroutine colorCoroutine in this.colorCoroutines)
            {
                if (colorCoroutine != null)
                {
                    StopCoroutine(colorCoroutine);
                }
            }
            this.colorCoroutines.Clear();
        }
        for (int i = 0; i < this.objectSpriteRenderer.Count; i++)
        {
            if (color == Color.white)
            {
                // If the given color is white, it means it's going to reset to the object's original color
                this.colorCoroutines.Add(StartCoroutine(ColorOverTimeCoroutine(this.objectSpriteRenderer[i], time, this.objectOriginalColor[i])));

            }
            else
            {
                this.colorCoroutines.Add(StartCoroutine(ColorOverTimeCoroutine(this.objectSpriteRenderer[i], time, color)));
            }

        }

        //StartCoroutine(ColorOverTimeCoroutine(time, color));
    }
    IEnumerator ColorOverTimeCoroutine(SpriteRenderer sRenderer, float time, Color color)
    {
        time = 0.5f;
        /*foreach (SpriteRenderer spriteRenderer in this.objectSpriteRenderer)
        {

            while (color != spriteRenderer.color)
            {
                spriteRenderer.color = Color.Lerp(spriteRenderer.color, color, Time.deltaTime / time);
                time -= Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }*/
        /*
        for (int i = 0; i < this.objectSpriteRenderer.Count; i++)
        {
            if (color == Color.white)
            {
                while (this.objectOriginalColor[i] != this.objectSpriteRenderer[i].color)
                {
                    this.objectSpriteRenderer[i].color = Color.Lerp(this.objectSpriteRenderer[i].color, this.objectOriginalColor[i], Time.deltaTime / time);
                    time -= Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                while (color != this.objectSpriteRenderer[i].color)
                {
                    this.objectSpriteRenderer[i].color = Color.Lerp(this.objectSpriteRenderer[i].color, color, Time.deltaTime / time);
                    time -= Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                }
            }

        }*/

        while (color != sRenderer.color)
        {
            sRenderer.color = Color.Lerp(sRenderer.color, color, Time.deltaTime / time);
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

}

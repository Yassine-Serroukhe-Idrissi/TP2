using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridShape : MonoBehaviour
{
    [FormerlySerializedAs("m_ShapePrefab")]
    [SerializeField]
    private GameObject shapePrefab;

    public Ex2Config config;

    public Color[,] Colors { get; private set; }

    private int _width;
    private int _height;
    private Queue<GameObject> _objectPool = new Queue<GameObject>();


    // Start is called before the first frame update
    private void Start()
    {
        var size = (float)config.nbCircles;
        var ratio = Camera.main!.aspect;
        _height = (int)Math.Round(Math.Sqrt(size / ratio));
        _width = (int)Math.Round(size / _height);

        Colors = new Color[_width, _height];
        var bottomLeftCorner = new Vector3(-_width / 2.0f, -_height / 2.0f, 0);
        var halfHeight = _height / 2f;
        var invWidth = 1f / _width;
        var invHeight = 1f / _height;

        for (int k = 0; k < _width * _height; k++)
        {
            var obj = Instantiate(shapePrefab);
            obj.SetActive(false); // Désactive l’objet pour éviter de le dessiner inutilement
            _objectPool.Enqueue(obj);
        }

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                var r = i * invWidth;
                var g = Mathf.Abs(j - halfHeight) * invHeight;
                var b = r * g;
                Colors[i, j] = new Color(r, g, b);

                // Récupère un objet du pool au lieu de l’instancier
                var shape = _objectPool.Dequeue();
                shape.SetActive(true);
                shape.transform.position = bottomLeftCorner + new Vector3(i, j, 0);

                var circle = shape.GetComponent<Circle>();
                circle.i = i;
                circle.j = j;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateColors();
    }

    private void UpdateColors()
    {
        var colorBuffer = new Color[_width];

        for (var j = 0; j < _height - 1; j++)
        {
            for (var i = 0; i < _width; i++)
            {
                colorBuffer[i] = Colors[i, j + 1];
            }

            for (var i = 0; i < _width; i++)
            {
                Colors[i, j + 1] = Colors[i, j];
                Colors[i, j] = colorBuffer[i];
            }
        }
    }
}
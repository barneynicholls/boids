//
// Boids - Flocking behavior simulation.
//
// Copyright (C) 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Collections;
using UnityEngine;
public class BoidControllerWithTarget : MonoBehaviour
{
    [HideInInspector]
    public Vector3 target = Vector3.zero;

    [Header("Target")]
    [SerializeField]
    private float targetRange = 20f;

    [SerializeField]
    private float minTargetChange = 3.0f;
    [SerializeField]
    private float maxTargetChange = 10.0f;

    [Header("Spawn")]
    [SerializeField]
    private GameObject boidPrefab;

    [SerializeField]
    private int spawnCount = 10;

    [SerializeField]
    private float spawnRadius = 4.0f;

    [Header("Behaviour")]

    [Range(0.1f, 20.0f)]
    public float velocity = 6.0f;

    [Range(0.0f, 0.9f)]
    public float velocityVariation = 0.5f;

    [Range(0.1f, 20.0f)]
    public float rotationCoeff = 4.0f;

    [Range(0.1f, 10.0f)]
    public float neighborDist = 2.0f;

    public LayerMask searchLayer;

    void Start()
    {
        for (var i = 0; i < spawnCount; i++) Spawn();

        StartCoroutine(NewTarget());
    }

    IEnumerator NewTarget()
    {
        target = new Vector3(
            transform.position.x + (Random.Range(-targetRange, targetRange)),
            transform.position.y + (Random.Range(-targetRange, targetRange)),
            transform.position.z + (Random.Range(-targetRange, targetRange)));

        yield return new WaitForSeconds(Random.Range(minTargetChange, maxTargetChange));

        StartCoroutine(NewTarget());
    }

    public GameObject Spawn()
    {
        return Spawn(transform.position + Random.insideUnitSphere * spawnRadius);
    }

    public GameObject Spawn(Vector3 position)
    {
        var rotation = Quaternion.Slerp(transform.rotation, Random.rotation, 0.3f);
        var boid = Instantiate(boidPrefab, position, rotation);
        boid.GetComponent<BoidBehaviourWithTarget>().controller = this;
        return boid;
    }
}

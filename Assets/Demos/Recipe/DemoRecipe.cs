using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class TestBase {
    public string name;
}

[Serializable]
public class TestDerivedFoo : TestBase {
    public float value;
}

[Serializable]
public class TestDerivedBar : TestBase {
    public int count;
}

public class DemoRecipe : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    public List<TestBase> Value;
}

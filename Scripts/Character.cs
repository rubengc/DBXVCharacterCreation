using System;
using UnityEngine;
using System.Collections;

/*
 * DBXV Character Creation
 *
 * The MIT License (MIT)
 *
 * Copyright (c) 2015 Ruben Garcia <rubengcdev@gmail.com> <rubengc.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 * Dragon Ball Xenoverse and all their content used on this project are property of BIRD STUDIO / SHUEISHA, TOEI ANIMATION.
 */

public class Character : MonoBehaviour {

    [Serializable]
    public class Colors {
        public Color hair = new Color(1, 1, 1, 1);
        public Color skin = new Color(1, 1, 1, 1);

        // Eyes use a custom shader based on Transparent/Diffuse assigning an alpha color
        public Color eye_color = new Color(1, 1, 1, 1);

        public Color bust_color_1 = new Color(1, 1, 1, 1);
        public Color bust_color_2 = new Color(1, 1, 1, 1);
        public Color bust_color_3 = new Color(1, 1, 1, 1);
        public Color bust_color_4 = new Color(1, 1, 1, 1);

        public Color rist_color_1 = new Color(1, 1, 1, 1);
        public Color rist_color_2 = new Color(1, 1, 1, 1);

        public Color pants_color_1 = new Color(1, 1, 1, 1);
        public Color pants_color_2 = new Color(1, 1, 1, 1);
        public Color pants_color_3 = new Color(1, 1, 1, 1);

        public Color boots_color_1 = new Color(1, 1, 1, 1);
        public Color boots_color_2 = new Color(1, 1, 1, 1);
        public Color boots_color_3 = new Color(1, 1, 1, 1);
    }

    public Global global;
    public Colors colors = new Colors();

    void Start () {
        // Initializes character parts
        SetPart(global.face.hairs[0], "Hair");
        SetPart(global.face.bases[0], "Face_base");
        SetPart(global.face.ears[0], "Face_ear");
        SetPart(global.face.eyes[0], "Face_eye");
        SetPart(global.face.foreheads[0], "Face_forehead");
        SetPart(global.face.noses[0], "Face_nose");

        SetPart(global.outfits[0], "Bust");
        SetPart(global.outfits[0], "Rist");
        SetPart(global.outfits[0], "Pants");
        SetPart(global.outfits[0], "Boots");
    }
	
	void Update () {

    }

    // Set a character GameObject part by name
    // If gO is null, will remove the part
    public void SetPart(GameObject gO, string name) {
        if(gO != null) {
            if(transform.Find(name) != null)
                DestroyImmediate(transform.Find(name).gameObject);

            GameObject clone = Instantiate(gO) as GameObject;

            string originalName = clone.name;

            clone.name = name;
            clone.transform.parent = transform;

            clone.transform.localRotation = Quaternion.Euler(0, 0, 0);
            if(originalName.Contains("Outfit")) { 
                if(!(originalName.Contains("Outfit000") || originalName.Contains("Outfit300")))
                    clone.transform.localRotation = Quaternion.Euler(270, 0, 0);
            }

            foreach(Transform child in clone.transform) {
                child.gameObject.SetActive(child.name.Contains(name));

                if(child.name.Contains(name))
                    child.localScale = new Vector3(30, 30, 30);
            }

            UpdateMaterialsColors();
        } else {
            if(transform.Find(name) != null) {
                Destroy(transform.Find(name).gameObject);
            }
        }
    }

    // Assign a color to the character colors
    public void SetColor(Color color, string name) {
        if(name.ToLower().Contains("hair"))
            colors.hair = color;
        else if(name.ToLower().Contains("skin"))
            colors.skin = color;
        else if(name.ToLower().Contains("eye_color"))
            colors.eye_color = color;
        else if(name.ToLower().Contains("bust_color_1"))
            colors.bust_color_1 = color;
        else if(name.ToLower().Contains("bust_color_2"))
            colors.bust_color_2 = color;
        else if(name.ToLower().Contains("bust_color_3"))
            colors.bust_color_3 = color;
        else if(name.ToLower().Contains("bust_color_4"))
            colors.bust_color_4 = color;
        else if(name.ToLower().Contains("rist_color_1"))
            colors.rist_color_1 = color;
        else if(name.ToLower().Contains("rist_color_2"))
            colors.rist_color_2 = color;
        else if(name.ToLower().Contains("pants_color_1"))
            colors.pants_color_1 = color;
        else if(name.ToLower().Contains("pants_color_2"))
            colors.pants_color_2 = color;
        else if(name.ToLower().Contains("pants_color_3"))
            colors.pants_color_3 = color;
        else if(name.ToLower().Contains("boots_color_1"))
            colors.boots_color_1 = color;
        else if(name.ToLower().Contains("boots_color_2"))
            colors.boots_color_2 = color;
        else if(name.ToLower().Contains("boots_color_3"))
            colors.boots_color_3 = color;

        UpdateMaterialsColors();
    }

    // Update all materials colors
    public void UpdateMaterialsColors() {
        foreach(Transform child in transform.GetComponentsInChildren<Transform>()) {
            if(child.GetComponent<Renderer>()) {
                foreach(Material material in child.GetComponent<Renderer>().materials) {
                    if(material.name.ToLower().Contains("hair"))
                        material.color = colors.hair;
                    else if(material.name.ToLower().Contains("skin"))
                        material.color = colors.skin;
                    else if(material.name.ToLower().Contains("eye_color"))
                        material.color = colors.eye_color;
                    else if(material.name.ToLower().Contains("bust_color_1"))
                        material.color = colors.bust_color_1;
                    else if(material.name.ToLower().Contains("bust_color_2"))
                        material.color = colors.bust_color_2;
                    else if(material.name.ToLower().Contains("bust_color_3"))
                        material.color = colors.bust_color_3;
                    else if(material.name.ToLower().Contains("bust_color_4"))
                        material.color = colors.bust_color_4;
                    else if(material.name.ToLower().Contains("rist_color_1"))
                        material.color = colors.rist_color_1;
                    else if(material.name.ToLower().Contains("rist_color_2"))
                        material.color = colors.rist_color_2;
                    else if(material.name.ToLower().Contains("pants_color_1"))
                        material.color = colors.pants_color_1;
                    else if(material.name.ToLower().Contains("pants_color_2"))
                        material.color = colors.pants_color_2;
                    else if(material.name.ToLower().Contains("pants_color_3"))
                        material.color = colors.pants_color_3;
                    else if(material.name.ToLower().Contains("boots_color_1"))
                        material.color = colors.boots_color_1;
                    else if(material.name.ToLower().Contains("boots_color_2"))
                        material.color = colors.boots_color_2;
                    else if(material.name.ToLower().Contains("boots_color_3"))
                        material.color = colors.boots_color_3;
                }
            }
        }
    }
}

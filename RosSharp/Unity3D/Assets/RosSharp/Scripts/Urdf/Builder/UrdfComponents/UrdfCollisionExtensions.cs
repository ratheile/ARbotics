﻿/*
© Siemens AG, 2018
Author: Suzannah Smith (suzannah.smith@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

<http://www.apache.org/licenses/LICENSE-2.0>.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using UnityEngine;

namespace RosSharp.Urdf.Editor
{
    public static class UrdfCollisionExtensions
    {
        public static void Create(Transform parent, GeometryTypes type, Transform visualToCopy = null)
        {
            GameObject collisionObject = new GameObject("unnamed");
            collisionObject.transform.SetParentAndAlign(parent);

            UrdfCollision urdfCollision = collisionObject.AddComponent<UrdfCollision>();
            urdfCollision.geometryType = type;

            if (visualToCopy != null)
            {
                if (urdfCollision.geometryType == GeometryTypes.Mesh)
                    UrdfGeometryCollision.CreateMatchingMeshCollision(collisionObject.transform, visualToCopy);
                else
                    UrdfGeometryCollision.Create(collisionObject.transform, type);

                //copy transform values from corresponding UrdfVisual
                collisionObject.transform.position = visualToCopy.position;
                collisionObject.transform.localScale = visualToCopy.localScale;
                collisionObject.transform.rotation = visualToCopy.rotation;
            }
            else
                UrdfGeometryCollision.Create(collisionObject.transform, type);
            
        }

        public static void Create(Transform parent, Link.Collision collision)
        {
            if (String.IsNullOrEmpty(collision.name)) collision.name = collision.GenerateNonReferenceID();

            if (parent.FindChildOrCreateWithComponent(collision.name, out GameObject collisionObject, out UrdfCollision urdfCollision))
            {
                urdfCollision.geometryType = UrdfGeometry.GetGeometryType(collision.geometry);

                UrdfGeometryCollision.Create(collisionObject.transform, urdfCollision.geometryType, collision.geometry);
                UrdfOrigin.ImportOriginData(collisionObject.transform, collision.origin);
            }
        }
    }
}
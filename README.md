Simulador Balístico en Unity

Un simulador donde el jugador controla un cañón y debe derribar estructuras de cubos.  
El disparo y las colisiones están gobernados por Rigidbody y el sistema de físicas de Unity.

Versión de Unity
Este proyecto fue desarrollado y probado en:

- Unity 6 (6000.0.46f1 LTS)
- Modo 3D

Cómo jugar
1. Ajusta los sliders en la interfaz:
   - Ángulo: controla la inclinación del cañón (arriba/abajo).
   - Dirección: controla la rotación horizontal (izquierda/derecha).
   - Fuerza: determina la potencia del disparo.
2. Selecciona la masa del proyectil en el menú desplegable.
3. Haz clic en el botón DISPARAR para lanzar el proyectil.
4. Observa cómo interactúa con la estructura de cubos y revisa el reporte de tiro.

Controles
- UI Sliders:  
  - Ángulo: inclinar cañón hacia arriba/abajo.  
  - Dirección: girar cañón hacia los costados.  
  - Fuerza: ajustar potencia del disparo.  

- Dropdown Masa: selecciona el peso del proyectil (afecta la física).  

- Botón Disparar: dispara un proyectil físico con las condiciones elegidas.  

Criterios de evaluación
El simulador cumple con los siguientes puntos:
- Controles de disparo en pantalla (sliders e inputs para ángulo, fuerza y masa).  
- Disparo físico (proyectiles con Rigidbody y Collider, lanzamiento con AddForce).  
- Escena de objetivos (estructuras armadas con Rigidbody y Joints estables al inicio).  
- Registro de resultados (se mide tiempo de vuelo, punto de impacto, velocidad, impulso de colisión y piezas derribadas).  
- Feedback al jugador (reporte de tiro con precisión y potencia).

LINK VIDEO DE YOUTUBE: https://youtu.be/sRW03kG3BeI

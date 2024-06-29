using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region GAME MANAGER
    public static GameManager Instance;

    private void Awake()
    {
        MakeSingleton();

        SceneManager.sceneLoaded += OnSceneLoaded;

        TestCarga();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        //TestCarga();
        InicializarJugadores();
        EscenaDeJuego();
        ResetiarVariables();
    }

    private void Update()
    {
        Update_Marcador_MHS();
    }

    private void FixedUpdate()
    {
        FixUpdate_Temporizador();
    }

    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion GAME MANAGER

    #region CARGAR ESCENA
    //En este metodo se pone todo lo que quieras que pase al cargar una escena
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetiarVariables();
        EscenaDeJuego();
    }

    void EscenaDeJuego()
    {
        if (SceneManager.GetActiveScene().name == "ANDYINGAME")
        {
            //Se crea una nueva partida de la clase partida y se le agrega el JugadoInfo correspondiente
            PartidaTest partida = new PartidaTest();
            partida.AgregarJugadorAlEquipo1(jugador1);
            partida.AgregarJugadorAlEquipo2(jugador2);


            IniciarPartida(partida);
            InicializarTemporizador();
            InicializarMapas();
            InicializarPuntaje();

            if (modoHS)
            {
                //Aqui ira todo lo que necesita el MHS
                InicializarMHS();
                InicializarMarcadorMHS();
                InicializarCamara();
                magosPrincipales.SetActive(true);
            }
            else if (modoDS)
            {
                //Aqui ira todo lo que necesita el MDS
                InicializarMDS();
                InicializarMarcadorMDS();
                InicializarCamara();
                pistaPintable.SetActive(true);
            }
        }
        else
        {
            return;
        }
    }

    private void ResetiarVariables()
    {
        if (SceneManager.GetActiveScene().name == "ANDYMENUTEST")
        {
            //Booleanos Partida
            modoHS = false;
            modoDS = false;
            boolMapaStreet = false;

            //Temporizador
            totalTime = 120;
            remainingTime = totalTime;
            isRunning = false;
            panelTiempoAgotado.SetActive(false);
            panelTemporizador.SetActive(false);

            //Paneles de condicion de Victoria
            panelVictoria.SetActive(false);

            //Mapas
            mapaStreet.SetActive(false);

            //Modo Hechizos Sazonados
            magosPrincipales.SetActive(false);
            panelMarcadorMHS.SetActive(false);
            camaraObjeto.SetActive(false);
            puntosAGanarTeam1 = puntajeInicial;
            puntosAGanarTeam2 = puntajeInicial;

            //Modo Duelo De Salsas
            pistaPintable.SetActive(false);
            panelMarcadorMDS.SetActive(false);
            camaraObjeto.SetActive(false);


        }
    }
    #endregion CARGAR ESCENA

    #region PARTIDA
    public bool _modoHS = modoHS;
    public bool _modoDS = modoDS;
    public static bool boolMapaStreet;
    public static bool modoHS;
    public static bool modoDS;

    private JugadorInfo jugador1;
    private JugadorInfo jugador2;
    private JugadorInfo jugador3;
    private JugadorInfo jugador4;

    public PartidaTest partida;

    public void TestCarga()
    {
        var gamepads = Gamepad.all;

        if (gamepads.Count < 2)
        {
            Debug.LogError("Se necesitan al menos 2 gamepads conectados.");
            return;
        }

        //Aqui creamos un nuevo JugadorInfo para cada control conectado
        jugador1 = new JugadorInfo(gamepads[0].deviceId, "J1");
        jugador2 = new JugadorInfo(gamepads[1].deviceId, "J2");


    }


    public void IniciarPartida(PartidaTest partida)
    {
        if (SceneManager.GetActiveScene().name != "ANDYINGAME")
            return;

        this.partida = partida;

        if (equipo1 == null)
            equipo1 = new List<PlayerController>();
        if (equipo2 == null)
            equipo2 = new List<PlayerController>();

        jugadores = new List<PlayerController>();

        if (partida.Equipo1.Count == 1)
        {
            p1 = SpawnJugador(partida.Equipo1[0], modo1v1spawn1);
            p1.equipo = 1;
            equipo1.Add(p1);
            jugadores.Add(p1);
            p2 = SpawnJugador(partida.Equipo2[0], modo1v1spawn2);
            equipo2.Add(p2);
            p2.equipo = 2;
            jugadores.Add(p2);
        }
        else
        {
            p1 = SpawnJugador(partida.Equipo1[0], modo2v2spawn1);
            p2 = SpawnJugador(partida.Equipo1[1], modo2v2spawn2);
            p3 = SpawnJugador(partida.Equipo2[0], modo2v2spawn3);
            p4 = SpawnJugador(partida.Equipo2[1], modo2v2spawn4);

            equipo1.Add(p1);
            equipo1.Add(p2);
            equipo2.Add(p3);
            equipo2.Add(p4);

        }

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("ANDYMENUTEST");
    }
    #endregion PARTIDA

    #region MAPAS
    [Header("Mapas")]
    [SerializeField] private GameObject mapaStreet;

    void InicializarMapas()
    {
        if (boolMapaStreet)
        {
            mapaStreet.SetActive(true);
        }
    }
    #endregion MAPAS

    #region PAUSA
    private static bool enPausa;
    public static bool EnPausa => enPausa;

    public static void Pausa(PlayerController player)
    {
        //Se invierten entre ellos
        enPausa = !enPausa;

        if (enPausa)
        {
            Time.timeScale = 0;

            foreach (PlayerController j in jugadores)
            {
                j.BloquearMovimiento = true;
            }
        }
        else
        {
            Time.timeScale = 1;

            foreach (PlayerController j in jugadores)
            {
                j.BloquearMovimiento = false;
            }

        }

    }
    #endregion PAUSA

    #region PUNTAJE
    [Header("Puntaje Core")]
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private TMP_Text playerWinText;
    [SerializeField] private int puntosAGanarTeam1;
    [SerializeField] private int puntosAGanarTeam2;
    private int puntajeInicial = 0;

    [Header("Puntaje MHS")]
    [SerializeField] private TMP_Text puntajeTeam1MHS;
    [SerializeField] private TMP_Text puntajeTeam2MHS;

    [Header("Puntaje MDS")]
    [SerializeField] private TMP_Text puntajeTeam1MDS;
    [SerializeField] private TMP_Text puntajeTeam2MDS;
    void InicializarPuntaje()
    {
        puntajeTeam1MHS.text = puntosAGanarTeam1.ToString();
        puntajeTeam2MHS.text = puntosAGanarTeam2.ToString();

        puntajeTeam1MDS.text = puntosAGanarTeam1.ToString();
        puntajeTeam2MDS.text = puntosAGanarTeam2.ToString();

        puntosAGanarTeam1 = puntajeInicial;
        puntosAGanarTeam2 = puntajeInicial;
    }
    #endregion PUNTAJE

    #region MARCADOR MHS
    [Header("Marcador Modo HS")]
    [SerializeField] private GameObject panelMarcadorMHS;
    [SerializeField] private TMP_Text rondaText;
    [SerializeField] private int numeroDeRonda;
    private int rondaInicial = 1;

    void InicializarMarcadorMHS()
    {
        panelVictoria.SetActive(false);
        panelMarcadorMHS.SetActive(true);
        numeroDeRonda = rondaInicial;
        rondaText.text = numeroDeRonda.ToString();
    }

    void Update_Marcador_MHS()
    {
        if (SceneManager.GetActiveScene().name == "ANDYINGAME" && modoHS)
        {
            if (puntosAGanarTeam1 >= 3 && isRunning)
            {
                isRunning = false;
                panelVictoria.SetActive(true);
                playerWinText.text = p1.gameObject.name;
                p1.enabled = false;
                p2.enabled = false;
                mago1Animator.SetTrigger("victoria");
                mago2Animator.SetTrigger("derrota");
            }

            else if (puntosAGanarTeam2 >= 3 && isRunning)
            {
                isRunning = false;
                panelVictoria.SetActive(true);
                playerWinText.text = p2.gameObject.name;
                p1.enabled = false;
                p2.enabled = false;
                mago2Animator.SetTrigger("victoria");
                mago1Animator.SetTrigger("derrota");
            }
        }
        else return;

    }
    #endregion MARCADOR MHS

    #region MARCADOR MDS
    [Header("Marcador Modo DS")]
    [SerializeField] private GameObject panelMarcadorMDS;

    void InicializarMarcadorMDS()
    {
        panelVictoria.SetActive(false);
        panelMarcadorMDS.SetActive(true);
    }
    #endregion MARCADOR MDS

    #region TEMPORIZADOR
    [Header("Temporizador")]
    [SerializeField] private GameObject panelTemporizador;
    [SerializeField] private GameObject panelTiempoAgotado;
    [SerializeField] private float totalTime = 120f; // Total del tiempo en segundos, 120 segundos es igual a 2 minutos
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text ganadorTiempoAgotadoText;

    private float remainingTime;
    private bool isRunning = false;

    private void InicializarTemporizador()
    {
        panelTemporizador.SetActive(true);
        panelTiempoAgotado.SetActive(false);
        remainingTime = totalTime;
        isRunning = true;
        Start_TimerText();
    }

    private void FixUpdate_Temporizador()
    {
        if (SceneManager.GetActiveScene().name == "ANDYINGAME")
        {
            if (isRunning)
            {
                timerText.text = remainingTime.ToString();
                remainingTime -= Time.fixedDeltaTime;
            }

            if (remainingTime <= 0)
            {
                isRunning = false;
                TimerEnded();

                if (modoHS)
                {
                    if (p1.salud < p2.salud)
                    {
                        puntosAGanarTeam2++;
                        CambioDeRondaMHS();

                    }
                    else
                    {
                        puntosAGanarTeam1++;
                        CambioDeRondaMHS();
                    }

                }
                else if (modoDS)
                {
                    TiempoAgotadoMDS();
                }

            }
        }
        else
        {
            return;
        }

    }

    private void TimerEnded()
    {
        timerText.text = "00:00";
    }

    private void Start_TimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion TEMPORIZADOR

    #region PERSONAJES
    [Header("Personajes")]
    [SerializeField] private PlayerController pfPersonaje1;
    [SerializeField] private PlayerController pfPersonaje2;
    [SerializeField] private PlayerController pfPersonaje3;
    [SerializeField] private PlayerController pfPersonaje4;
    #endregion PERSONAJES

    #region JUGADORES
    private static List<PlayerController> jugadores;

    [Header("Spawn Points")]
    [SerializeField] private Transform modo1v1spawn1;
    [SerializeField] private Transform modo1v1spawn2;
    [SerializeField] private Transform modo2v2spawn1;
    [SerializeField] private Transform modo2v2spawn2;
    [SerializeField] private Transform modo2v2spawn3;
    [SerializeField] private Transform modo2v2spawn4;

    private static List<PlayerController> equipo1;
    private static List<PlayerController> equipo2;

    private static PlayerController p1, p2, p3, p4;

    void InicializarJugadores()
    {
        equipo1 = new List<PlayerController>();
        equipo2 = new List<PlayerController>();
    }

    private PlayerController SpawnJugador(JugadorInfo jugadorInfo, Transform spawnPoint)
    {
        PlayerController personaje = null;

        switch (jugadorInfo.Personaje)
        {
            case "J1": personaje = pfPersonaje1; break;
            case "J2": personaje = pfPersonaje2; break;
            case "J3": personaje = pfPersonaje3; break;
            case "J4": personaje = pfPersonaje4; break;
            default:
                Debug.LogError("Personaje no reconocido: " + jugadorInfo.Personaje);
                break;
        }

        if (personaje != null)
        {
            return Instantiate(personaje, spawnPoint.position, spawnPoint.rotation);
        }

        return null;
    }


    #endregion JUGADORES

    #region CAMARA
    [Header("Camara Principal De Juego")]
    [SerializeField] private Camera camaraPrincipal;
    [SerializeField] private GameObject camaraObjeto;

    void InicializarCamara()
    {
        camaraObjeto = camaraPrincipal.gameObject;
        camaraObjeto.SetActive(true);
        //originalCameraPosition = camaraPrincipal.transform.position;
    }
    #endregion CAMARA

    #region MODO DUELO DE SALSAS
    [Header("Modo Duelo De Salsas")]

    [Header("Pista")]
    [SerializeField] private GameObject pistaPintable;
    [SerializeField] private List<CuadroPintable> cuadrosPintables;
    private List<CuadroPintable> cuadrosJ1;
    private List<CuadroPintable> cuadrosJ2;

    void InicializarMDS()
    {
        camaraObjeto = camaraPrincipal.gameObject;
        camaraObjeto.SetActive(true);
        panelMarcadorMDS.SetActive(true);
        cuadrosJ1 = new List<CuadroPintable>();
        cuadrosJ2 = new List<CuadroPintable>();
    }

    public void RegistrarCuadroPintado(CuadroPintable cuadro)
    {
        //Verificamos que nombre nos llamo el cuadro
        if (cuadro.currentOwner == "J1(Clone)")
        {
            //Si no se encuentra en la lista el cuadro mandado, lo agregamos
            if (!cuadrosJ1.Contains(cuadro))
            {
                Debug.Log("Cuadro registrado a J1");
                puntosAGanarTeam1++;
                puntajeTeam1MDS.text = puntosAGanarTeam1.ToString();
                cuadrosJ1.Add(cuadro);
            }

        }
        else if (cuadro.currentOwner == "J2(Clone)")
        {
            if (!cuadrosJ2.Contains(cuadro))
            {
                Debug.Log("Cuadro registrado a J2");
                puntosAGanarTeam2++;
                puntajeTeam2MDS.text = puntosAGanarTeam2.ToString();
                cuadrosJ2.Add(cuadro);
            }

        }
        else
        {
            Debug.LogError("No se registro ningun cuadro a ningun Jugador");
        }

    }

    public void RemoverCuadroPintado(CuadroPintable cuadro, string owner)
    {
        if (owner == "J1(Clone)" && cuadrosJ1.Contains(cuadro))
        {
            puntosAGanarTeam1--;
            puntajeTeam1MDS.text = puntosAGanarTeam1.ToString();
            cuadrosJ1.Remove(cuadro);
        }
        else if (owner == "J2(Clone)" && cuadrosJ2.Contains(cuadro))
        {
            puntosAGanarTeam2--;
            puntajeTeam2MDS.text = puntosAGanarTeam2.ToString();
            cuadrosJ2.Remove(cuadro);
        }
    }

    //Metodo para verifica que lista tiene mas cuadros pintados cuanso acabe el tiempo
    private void TiempoAgotadoMDS()
    {
        int j1Count = cuadrosJ1.Count;
        int j2Count = cuadrosJ2.Count;

        if (j1Count > j2Count)
        {
            panelVictoria.SetActive(true);
            playerWinText.text = p1.gameObject.name;
        }
        else if (j2Count > j1Count)
        {
            panelVictoria.SetActive(true);
            playerWinText.text = p2.gameObject.name;
        }
    }
    #endregion MODO DUELO DE SALSAS

    #region MODO HECHIZOS SAZONADOS
    [Header("Modo Hechizos Sazonados")]
    [SerializeField] private GameObject magosPrincipales;

    [Header("Magos")]
    [SerializeField] private GameObject mago1;
    [SerializeField] private GameObject mago2;
    private Animator mago1Animator;
    private Animator mago2Animator;

    [Header("Cámara Stats MH")]
    [SerializeField] private Transform posicionCinematica;
    [SerializeField] private float velocidadMovCamara = 2f;
    private Animator camaraPrincipalAnimator;

    private Vector3 originalCameraPosition; // Posición original de la cámara

    [Header("Hechizo")]
    [SerializeField] private GameObject hechizoPrefab;
    [SerializeField] private float hechizoVelocidad = 10f;
    [SerializeField] private Transform spawnHechizo1;
    [SerializeField] private Transform spawnHechizo2;

    void InicializarMHS()
    {
        panelMarcadorMHS.SetActive(true);
        mago1Animator = mago1.GetComponent<Animator>();
        mago2Animator = mago2.GetComponent<Animator>();
        camaraPrincipalAnimator = camaraPrincipal.GetComponent<Animator>();
        mago1Animator.SetBool("idle", true);
        mago2Animator.SetBool("idle", true);
    }

    //Parte de los Magos
    void Mago1()
    {
        Debug.Log("Si se intancio el hechizo del mago 1");
        GameObject hechizo = Instantiate(hechizoPrefab, spawnHechizo1.transform.position, Quaternion.identity);
        mago1Animator.SetTrigger("ataque");
        StartCoroutine(MoverHechizo1(hechizo));
    }

    void Mago2()
    {
        Debug.Log("Si se intancio el hechizo del mago 2");
        GameObject hechizo = Instantiate(hechizoPrefab, spawnHechizo2.transform.position, Quaternion.identity);
        mago2Animator.SetTrigger("ataque");
        StartCoroutine(MoverHechizo2(hechizo));
    }

    IEnumerator MoverHechizo1(GameObject hechizo)
    {
        while (hechizo != null)
        {
            hechizo.transform.position = Vector3.MoveTowards(hechizo.transform.position, mago2.transform.position, 10f * Time.deltaTime);

            if (Vector3.Distance(hechizo.transform.position, mago2.transform.position) < 0.1f)
            {
                mago2Animator.SetTrigger("daño");
                Destroy(hechizo);
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator MoverHechizo2(GameObject hechizo)
    {
        while (hechizo != null)
        {
            hechizo.transform.position = Vector3.MoveTowards(hechizo.transform.position, mago1.transform.position, 10f * Time.deltaTime);

            if (Vector3.Distance(hechizo.transform.position, mago1.transform.position) < 0.1f)
            {
                mago1Animator.SetTrigger("daño");
                Destroy(hechizo);
                yield break;
            }

            yield return null;
        }
    }

    ////Parte de la Camara
    //public void PosicionCamaraCinematica()
    //{
    //    StartCoroutine(MoverCamara(camaraPrincipal.transform, posicionCinematica.position));
    //}

    //IEnumerator MoverCamara(Transform camaraTransform, Vector3 targetPosition)
    //{
    //    while (Vector3.Distance(camaraTransform.position, targetPosition) > 0.1f)
    //    {
    //        camaraTransform.position = Vector3.Lerp(camaraTransform.position, targetPosition, velocidadMovCamara * Time.deltaTime);
    //        yield return null;
    //    }

    //    // Asegurarse de que la posición final sea exacta.
    //    camaraTransform.position = targetPosition;

    //    // Regresar la cámara a su posición original
    //    StartCoroutine(RetornarCamara(3.0f));
    //}

    //void PosicionCamaraPrincipal()
    //{
    //    StartCoroutine(MoverCamara(camaraPrincipal.transform, originalCameraPosition));
    //}

    //IEnumerator RetornarCamara(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    PosicionCamaraPrincipal();
    //}
    #endregion MODO HECHIZOS SAZONADOS

    #region DEAD EVENT PLAYER MHS
    //Esto se manda a llamar cada vez que un jugador muere
    public void DeadPlayerEventMHS(PlayerController player)
    {
        if (modoHS)
        {
            if (puntosAGanarTeam1 != 3 || puntosAGanarTeam2 != 3)
            {
                if (player.gameObject.name == "J1(Clone)" && isRunning)
                {
                    isRunning = false;
                    p1.BloquearMovimiento = true;
                    p2.BloquearMovimiento = true;
                    puntosAGanarTeam2++;
                    puntajeTeam2MHS.text = puntosAGanarTeam2.ToString();
                    //Invoke("PosicionCamaraCinematica", 1.50f);
                    camaraPrincipalAnimator.SetTrigger("move");
                    Invoke("Mago2", 3.0f);
                    Invoke("CambioDeRondaMHS", 6.0f);
                }
                else if (player.gameObject.name == "J2(Clone)" && isRunning)
                {
                    isRunning = false;
                    p1.BloquearMovimiento = true;
                    p2.BloquearMovimiento = true;
                    puntosAGanarTeam1++;
                    puntajeTeam1MHS.text = puntosAGanarTeam1.ToString();
                    //Invoke("PosicionCamaraCinematica", 1.50f);
                    camaraPrincipalAnimator.SetTrigger("move");
                    Invoke("Mago1", 3.0f);
                    Invoke("CambioDeRondaMHS", 6.0f);
                }
            }
            else
            {
                return;
            }

        }

        else if (modoDS)
        {
            if (player.gameObject.name == "J1(Clone)")
            {
                p1.BloquearMovimiento = true;
                RespawnearJugadorMDS(player);
            }
            else if (player.gameObject.name == "J2(Clone)")
            {
                p2.BloquearMovimiento = true;
                RespawnearJugadorMDS(player);
            }
        }
        else
        {
            return;
        }

    }


    private void CambioDeRondaMHS()
    {
        //Ajustamos el numero de ronda
        numeroDeRonda++;
        rondaText.text = numeroDeRonda.ToString();

        //Desactivamos los pefabs de los jugadores
        p1.gameObject.SetActive(false);
        p2.gameObject.SetActive(false);

        //Los movemos a sus posiciones iniciales
        p1.transform.position = modo1v1spawn1.transform.position;
        p2.transform.position = modo1v1spawn2.transform.position;


        //Los reactivamos
        StartCoroutine(ReactivacionMHS(3.0f));

    }

    IEnumerator ReactivacionMHS(float time)
    {
        p1.salud = 100;
        p2.salud = 100;
        p1.enabled = true;
        p2.enabled = true;
        p1.isInvulnerable = false; p2.isInvulnerable = false;
        p1.gameObject.SetActive(true);
        p2.gameObject.SetActive(true);
        p1.anim.SetTrigger("spawn");
        p2.anim.SetTrigger("spawn");
        yield return new WaitForSeconds(time);
        p1.BloquearMovimiento = false;
        p2.BloquearMovimiento = false;
        isRunning = true;
        remainingTime = totalTime;
        p1.muerto = false; p2.muerto = false;
        yield return new WaitForSeconds(3.50f);
    }

    void RespawnearJugadorMDS(PlayerController player)
    {
        if (player.gameObject.name == "J1(Clone)")
        {
            p1.muerto = true;
            p1.BloquearMovimiento = true;

            StartCoroutine(ReactivacionMDS(player, 8.0f));
        }
        else if (player.gameObject.name == "J2(Clone)")
        {

            p2.muerto = true;
            p2.BloquearMovimiento = true;

            StartCoroutine(ReactivacionMDS(player, 8.0f));
        }
    }

    IEnumerator ReactivacionMDS(PlayerController player, float time)
    {
        if (player.gameObject.name == "J1(Clone)")
        {
            yield return null;
            yield return new WaitForSeconds(6.0f);
            p1.gameObject.SetActive(false);
            p1.transform.position = modo1v1spawn1.transform.position;
            p1.salud = 100;
            p1.enabled = true;
            p1.isInvulnerable = false;
            p1.gameObject.SetActive(true);
            p1.anim.SetTrigger("spawn");
            p1.BloquearMovimiento = false;
            p1.muerto = false;
            yield return new WaitForSeconds(time);
        }
        else if (player.gameObject.name == "J2(Clone)")
        {
            yield return null;
            yield return new WaitForSeconds(6.0f);
            p2.gameObject.SetActive(false);
            p2.transform.position = modo1v1spawn2.transform.position;
            p2.salud = 100;
            p2.enabled = true;
            p2.isInvulnerable = false;
            p2.gameObject.SetActive(true);
            p2.anim.SetTrigger("spawn");
            p2.BloquearMovimiento = false;
            p2.muerto = false;
            yield return new WaitForSeconds(time);
        }


    }
    #endregion DEAD EVENT PLAYER MHS


}

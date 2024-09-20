using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataPopulator
{
    internal static class BooksToPopulate
    {
        internal static (string language, string title, string? Uri, string text)[] books = [

("Spanish", "Rapunzel", null,
@"Había una vez una pareja que quería un hijo. La esposa, que estaba esperando, tenía antojo de una planta llamada rapunzel, que crecía en un jardín cercano perteneciente a una hechicera.
El esposo, queriendo complacer a su esposa, se coló en el jardín para conseguir la rapunzel. Sin embargo, fue descubierto por la hechicera. Ella accedió a dejarlo llevar tanta rapunzel como su esposa quisiera, pero a cambio, debía prometer darle el hijo una vez que naciera.
La pareja tuvo una niña, y la hechicera vino a reclamarla. Le puso el nombre de Rapunzel y la encerró en una torre sin puertas. La única entrada era una ventana en la parte superior, y la hechicera la visitaba llamándola: ""Rapunzel, Rapunzel, baja tu cabello"".
Rapunzel tenía cabello largo y hermoso, y la hechicera lo usaba como una escalera. Un día, un príncipe escuchó a Rapunzel cantar en la torre y quedó cautivado por su voz. Observó cómo la hechicera usaba el cabello de Rapunzel para subir y decidió intentarlo él mismo.
El príncipe aprendió las palabras mágicas y visitó a Rapunzel en secreto. Se enamoraron, y él prometió rescatarla. Rapunzel aceptó escapar con él cuando regresara.
Desafortunadamente, la hechicera descubrió su plan. Enojada, cortó el cabello de Rapunzel y la desterró al desierto. Cuando el príncipe vino a visitarla, la hechicera bajó el cabello cortado de Rapunzel, y él subió. Para su sorpresa, encontró a la hechicera en lugar de Rapunzel.
Desesperado, el príncipe saltó de la torre, y las espinas lo dejaron ciego. Vagó por el desierto buscando a Rapunzel. Finalmente, la encontró, y sus lágrimas de alegría curaron sus ojos. Vivieron felices para siempre.
Y así, el cuento de Rapunzel nos enseña sobre el amor, la lealtad y el poder de la bondad que triunfa sobre el mal.
"),

("Spanish", "Aladino y la lámpara mágica", null,
@"Aladino y la lámpara mágica
Había una vez, en una ciudad de Arabia, un joven llamado Aladino. Vivía con su madre en una pequeña casa. Eran pobres, pero Aladino siempre soñaba con ser rico.
Un día, un hombre extraño llegó a la ciudad. Este hombre era un mago malo. Dijo que era el tío de Aladino y lo llevó al desierto. Allí, el mago le mostró una cueva mágica.
—Aladino, entra en la cueva y trae la lámpara que está dentro—, dijo el mago.
Aladino entró en la cueva y encontró la lámpara. Pero cuando salió, el mago le pidió la lámpara antes de ayudarlo a salir de la cueva. Aladino no confió en él y no le dio la lámpara. Entonces, el mago, enojado, cerró la cueva, dejando a Aladino atrapado.
Aladino estaba triste y asustado. Mientras miraba la lámpara, la frotó por accidente. De repente, ¡un genio apareció!
—Soy el genio de la lámpara. ¿Cuál es tu deseo?—, dijo el genio.
Aladino pidió salir de la cueva, y el genio lo llevó a casa con su madre. Ahora, con la lámpara mágica, Aladino y su madre vivían mejor. Aladino pidió al genio muchos deseos: una casa grande, ropa bonita, y comida deliciosa.
Un día, Aladino vio a la princesa en el mercado y se enamoró de ella. Quería casarse con ella, así que pidió al genio que lo ayudara. El genio le dio regalos maravillosos para el sultán, y Aladino pudo casarse con la princesa.
Pero el mago malo regresó. Quería la lámpara, así que engañó a la princesa para que se la diera. Con la lámpara en sus manos, el mago usó el poder del genio para llevarse todo lo que Aladino tenía, incluso su palacio y la princesa.
Aladino estaba triste, pero no se rindió. Con la ayuda de un anillo mágico que también tenía un genio, logró encontrar al mago. Engañó al mago y recuperó la lámpara. Aladino volvió a pedir sus deseos al genio, y todo regresó a la normalidad.
Al final, Aladino y la princesa vivieron felices para siempre, y nunca más usaron la lámpara mágica.
"),

("Spanish", "Cenicienta", null,
@"Cenicienta
Había una vez una joven llamada Cenicienta. Ella vivía con su madrastra y dos hermanastras. Ellas eran muy crueles con Cenicienta y la hacían trabajar todo el día limpiando la casa.
Un día, el príncipe organizó un gran baile en el palacio. Todas las jóvenes del reino estaban invitadas. Las hermanastras de Cenicienta fueron al baile, pero no dejaron que Cenicienta fuera.
Cenicienta estaba muy triste. De repente, apareció su hada madrina. Con su varita mágica, transformó un calabaza en un bonito carruaje, a los ratones en caballos, y a Cenicienta le dio un vestido precioso y unos zapatos de cristal. Pero le advirtió que debía volver antes de la medianoche, porque a esa hora el hechizo se rompería.
Cenicienta fue al baile y el príncipe quedó enamorado de ella. Bailaron toda la noche, pero cuando el reloj marcó la medianoche, Cenicienta tuvo que salir corriendo. En su prisa, perdió uno de sus zapatos de cristal.
El príncipe, decidido a encontrarla, buscó por todo el reino a la dueña del zapato. Finalmente, llegó a la casa de Cenicienta. Las hermanastras intentaron ponerse el zapato, pero no les quedaba bien. Entonces, Cenicienta se lo probó, y le quedó perfecto.
El príncipe y Cenicienta se casaron y vivieron felices para siempre."),

("Spanish", "Ananse y la Olla de Sabiduría", null,
@"Ananse y la Olla de Sabiduría
Había una vez, en un pueblo lejano, una araña muy lista llamada Ananse. Un día, Ananse decidió que quería tener toda la sabiduría del mundo para él solo. Entonces, fue al bosque y encontró una olla mágica que contenía toda la sabiduría.
Ananse estaba muy feliz. Pensó: ""Voy a guardar esta olla para mí. Seré el más sabio de todos.""
Ananse llevaba la olla a casa, pero tenía miedo de que alguien más pudiera robarla. Entonces, decidió esconderla en lo alto de un árbol.
Ananse tomó la olla y empezó a subir al árbol. Pero tenía un problema: llevaba la olla en el frente, y eso hacía difícil subir. Ananse se resbalaba y no podía llegar a la cima.
Mientras Ananse intentaba subir, su hijo, que estaba mirando desde abajo, le dijo: ""Papá, ¿por qué no pones la olla en tu espalda? Será más fácil subir así.""
Ananse se enfadó. Pensó: ""¡Yo tengo toda la sabiduría! ¿Cómo es posible que mi hijo sepa algo que yo no sé?""
Pero Ananse decidió probar el consejo de su hijo. Puso la olla en su espalda y pudo subir el árbol fácilmente.
Cuando llegó a la cima, se dio cuenta de que aunque tenía la olla de sabiduría, no lo sabía todo. Entonces, Ananse entendió que la sabiduría no se puede guardar en una sola persona. Así que, en lugar de esconder la olla, la rompió y dejó que la sabiduría se esparciera por el mundo para que todos pudieran aprender.
"),

("Spanish", @"[GUION PARA VIDEO: EXPLICACIÓN DE ""COMPREHENSIBLE INPUT"" EN 5 MINUTOS]", null,
@"[GUION PARA VIDEO: EXPLICACIÓN DE ""COMPREHENSIBLE INPUT"" EN 5 MINUTOS]
[INTRO SEQUENCE]
Música animada, fondo colorido con el título ""¡Comprehensible Input: El MEJOR Método para Aprender un Idioma!""
[EN PANTALLA: Aparece el YouTuber sonriendo, de pie frente a un fondo simple con imágenes relacionadas con el aprendizaje de idiomas, como libros, nubes de palabras y banderas. Aparece el subtítulo ""Explicación de Comprehensible Input"".]
YouTuber
""¡Hola, qué tal, estudiantes de idiomas! Bienvenidos de nuevo al canal, donde hablamos de los MEJORES métodos para aprender idiomas rápido y de forma efectiva. Si eres nuevo aquí, no olvides darle al botón de suscribirse y activar las notificaciones porque hoy vamos a hablar de una de las herramientas más poderosas para aprender cualquier idioma: ¡comprehensible input!""
[SECCIÓN 1: ¿Qué es el Comprehensible Input?]
La música de fondo se desvanece mientras aparecen visuales de texto y símbolos que representan el habla.
YouTuber
""Bien, entonces, ¿qué es el comprehensible input o entrada comprensible? Puede que hayas escuchado este término si estás en el mundo del aprendizaje de idiomas. Es un método popularizado por el lingüista Stephen Krashen, que se basa en la idea de que aprendemos mejor un idioma cuando entendemos lo que estamos escuchando o leyendo, aunque no sepamos cada palabra.""
""El comprehensible input básicamente significa exponerse a un idioma que está un poco por encima de tu nivel actual. No demasiado fácil, porque no te desafiaría, pero tampoco tan difícil que te pierdas por completo. Piénsalo como escalones: necesitas contenido que te rete, pero que aún esté al alcance.""
[EN PANTALLA: Aparece un texto breve para enfatizar]
""¡Contenido que puedes entender en su mayoría, con algunas palabras y estructuras nuevas!""
[SECCIÓN 2: ¿Por qué es tan efectivo?]
La cámara hace un leve zoom para dar énfasis.
YouTuber
""Ahora, déjame explicarte por qué este método funciona tan bien. Cuando te expones a un idioma en su contexto—ya sea escuchando o leyendo—tu cerebro empieza a captar patrones de manera natural. Absorbes reglas gramaticales, estructuras de frases y vocabulario sin darte cuenta. En lugar de memorizar largas listas de palabras o reglas gramaticales, aprendes por experiencia.
¡Es como los niños aprenden su primer idioma! Ellos no estudian, ¿verdad? Solo escuchan mucho lenguaje a su alrededor, y poco a poco entienden más hasta que hablan con fluidez. Eso es exactamente lo que haces con el comprehensible input: exponerte al idioma de una manera que se siente natural.""
[SECCIÓN 3: ¿Cómo encontrar buen Comprehensible Input?]
La música animada vuelve mientras la escena cambia a mostrar varios formatos de medios (libros, videos, aplicaciones) en pantalla.
YouTuber
""Entonces, ¿cómo puedes encontrar tú comprehensible input para tu camino en el aprendizaje de idiomas? ¡Es más fácil de lo que piensas! Puedes usar todo tipo de materiales, siempre y cuando se ajusten a tu nivel:
Lecturas graduadas: Son libros escritos específicamente para estudiantes, con vocabulario y gramática ajustados a tu nivel.
Libros infantiles: ¡No los subestimes! Suelen ser simples y están llenos de lenguaje útil.
Podcasts o videos para principiantes: Hay muchísimos podcasts y canales de YouTube diseñados para enseñar a los principiantes a través de conversaciones reales.
Series con subtítulos: Encuentra programas o videos en YouTube en tu idioma objetivo que te gusten. Empieza con subtítulos en tu propio idioma, y luego cambia a subtítulos en el idioma que estás aprendiendo a medida que mejoras.
Aplicaciones de idiomas: Algunas aplicaciones ofrecen contenido adaptado a tu nivel, como ejercicios de escucha graduados o historias. Duolingo, LingQ y FluentU son grandes ejemplos.""
[EN PANTALLA: Aparecen ejemplos]
Lecturas Graduadas
Podcasts para Principiantes
Libros Infantiles
Series con Subtítulos
Aplicaciones de Idiomas (Duolingo, LingQ, FluentU)
YouTuber
""Recuerda, el objetivo es poder seguir el contenido la mayor parte del tiempo, aunque haya algunas cosas que todavía no entiendas. ¡Así es como progresas!""
[SECCIÓN 4: Mis Consejos Personales para Usar Comprehensible Input]
El YouTuber gesticula hacia la cámara, añadiendo un toque personal.
YouTuber
""Bien, aquí tienes algunos de mis consejos personales para sacar el máximo provecho del comprehensible input:
No te preocupes por entender todo. En serio, está bien si no entiendes cada palabra. Concéntrate en captar el significado general.
La repetición es clave. Si encuentras algo que te gusta y que entiendes en su mayoría, ¡escúchalo o léelo de nuevo! Captarás más cosas la segunda o tercera vez.
Elige contenido que te interese. Cuanto más disfrutes lo que estás aprendiendo, más tiempo pasarás haciéndolo. ¿Te gusta la cocina? Mira programas de cocina en tu idioma objetivo. ¿Te gustan los deportes? Sigue un podcast de deportes. ¡Hace que aprender sea mucho más divertido!
Fíjate metas pequeñas. En lugar de intentar ver una película entera, empieza con videos o podcasts cortos—de cinco o diez minutos. Ve aumentando gradualmente.""
[EN PANTALLA: Lista rápida de consejos para dar énfasis]
No te preocupes por entender todo
Repite contenido
Elige temas que disfrutes
Fíjate metas pequeñas
[CONCLUSIÓN: El Poder del Comprehensible Input]
La cámara se aleja nuevamente, mientras la música animada vuelve.
YouTuber
""Para resumir, el comprehensible input es una herramienta increíble para aprender idiomas. Te ayuda a absorber vocabulario y gramática de manera natural y, lo más importante, mantiene el proceso de aprendizaje divertido y motivador. ¡Así que sal ahí y comienza a escuchar, ver y leer en tu idioma objetivo!""
[EN PANTALLA: Aparece un texto alentador]
""¡Cuanto más te sumerjas, más aprenderás!""
YouTuber
""Si te ha gustado este video, dale un like y cuéntame en los comentarios qué idioma estás aprendiendo y qué materiales te han sido útiles. No olvides suscribirte para más consejos y trucos de aprendizaje de idiomas, ¡y nos vemos en el próximo video! ¡Hasta luego!""
[SECUENCIA FINAL]
La música se desvanece mientras aparece en la pantalla ""¡Gracias por ver! ¡Suscríbete para más!"""
),

("Spanish", "Golpe de Estado en España de 1981", "https://es.wikipedia.org/wiki/Golpe_de_Estado_en_Espa%C3%B1a_de_1981",@"
Golpe de Estado en España de 1981
El golpe de Estado de 1981, también conocido por el numerónimo 23F, fue un intento fallido de golpe de Estado perpetrado el lunes 23 de febrero de 1981 por algunos mandos militares en España. Los hechos principales sucedieron en las ciudades de Valencia y Madrid.
En Madrid, a las 18:23 horas, un numeroso grupo de guardias civiles a cuyo mando se encontraba el teniente coronel Antonio Tejero asaltó el Palacio de las Cortes durante la votación para la investidura del candidato a la Presidencia del Gobierno, Leopoldo Calvo-Sotelo, hasta entonces vicepresidente segundo del Gobierno y diputado de la Unión de Centro Democrático (UCD). Los diputados y el Gobierno de España al completo fueron secuestrados en su interior.
La ciudad de Valencia fue ocupada militarmente, en virtud del estado de excepción proclamado por el teniente general Jaime Milans del Bosch, capitán general de la III región militar. Dos mil hombres y cincuenta carros de combate fueron desplegados en las calles de la ciudad.
A las 21 horas de ese mismo día, el diario El País puso en la calle una edición especial posicionándose contra el golpe con el titular en primera plana: «El País, con la Constitución». A la una de la madrugada del 24 de febrero, el rey Juan Carlos I, vestido con uniforme de capitán general de los Ejércitos, se dirigió a la nación por televisión para situarse en contra de los golpistas y defender la Constitución española. Poco después, Milans dio la orden de regresar a sus unidades a los contingentes militares que ocupaban Valencia. El secuestro del Congreso terminó a mediodía del día 24.
El Tribunal Supremo, en casación, condenó a 30 años de cárcel a Milans, Tejero y Alfonso Armada como principales responsables del golpe de Estado. En total fueron condenados doce miembros de las Fuerzas Armadas, diecisiete miembros de la Guardia Civil y un civil. Menos Tejero, todos ellos salieron de la prisión antes del año 1990 (el general Armada gracias a un indulto).
Como ha destacado Juan Francisco Fuentes, «el estrepitoso fracaso del golpe trajo consigo la consolidación de una democracia tambaleante y la derrota definitiva del golpismo, del que puede decirse que ya nunca levantó cabeza».«El 23-F fue el resultado de una tradición militarista que murió con él», añade Fuentes.
"),

("Spanish", "España", "https://es.wikipedia.org/wiki/Espa%C3%B1a", @"
España, formalmente Reino de España, es un país soberano transcontinental, constituido en Estado social y democrático de derecho y cuya forma de gobierno es la monarquía parlamentaria. Es uno de los veintisiete Estados soberanos que forman la Unión Europea. Su territorio, con capital en Madrid, está organizado en diecisiete comunidades autónomas, formadas a su vez por cincuenta provincias, y dos ciudades autónomas.
España se sitúa principalmente en el suroeste de Europa, si bien también tiene presencia en el norte de África. En Europa, ocupa la mayor parte de la península ibérica, conocida como España peninsular, y las islas Baleares (en el mar Mediterráneo). En África se hallan las ciudades de Ceuta y Melilla, las islas Canarias (en el océano Atlántico) y varias posesiones mediterráneas denominadas «plazas de soberanía». El municipio de Llivia, en los Pirineos, constituye un exclave rodeado totalmente por territorio francés. Completa el conjunto de territorios una serie de islas e islotes frente a las propias costas peninsulares. Tiene una extensión de 505 370 km2, por lo que es el cuarto país más extenso del continente, y con una altitud media de 650 m sobre el nivel del mar, uno de los países más montañosos de Europa. Su población casi llega a los 48 millones y medio de habitantes, aunque la densidad de población es reducida si se compara con el contexto europeo. Concretamente, a 1 de julio de 2024 llegó hasta los 48 797 897. El territorio peninsular comparte fronteras terrestres con Francia y con Andorra al norte, con Portugal al oeste y con Gibraltar al sur. En sus territorios africanos, comparte fronteras terrestres y marítimas con Marruecos. Comparte con Francia la soberanía sobre la isla de los Faisanes en la desembocadura del río Bidasoa y cinco facerías pirenaicas.
El artículo 3.1 de su Constitución establece que «el castellano es la lengua española oficial del Estado. Todos los españoles tienen el deber de conocerla y el derecho a usarla». En 2012, era la lengua materna del 82 % de los españoles. Según el artículo 3.2, «las demás lenguas españolas serán también oficiales en las respectivas Comunidades Autónomas de acuerdo con sus Estatutos». El idioma español o castellano, segunda lengua materna más hablada del mundo con 500 millones de hispanohablantes nativos, y hasta casi los 600 millones incluyendo hablantes con competencia limitada, es uno de los más importantes legados del acervo cultural e histórico de España en el mundo. Perteneciente culturalmente a la Europa Latina y heredero de una vasta influencia grecorromana, España alberga también la cuarta colección más numerosa del mundo de sitios declarados Patrimonio de la Humanidad por la Unesco.
Es un país desarrollado —goza de la cuarta esperanza de vida más elevada del mundo— y de altos ingresos, cuyo producto interior bruto coloca a la economía española en la decimocuarta posición mundial (2021). España es una gran potencia turística, ya que destaca como el segundo país más visitado del mundo —más de 83 millones de turistas en 2019— y el segundo país del mundo en ingresos económicos provenientes del turismo internacional. Tiene un índice de desarrollo humano muy alto (0,911), según el informe de 2022 del Programa de la ONU para el Desarrollo. España también tiene una notable proyección internacional a través de su pertenencia a múltiples organizaciones internacionales como Naciones Unidas, el Consejo de Europa, la Organización Mundial del Comercio, la Organización de Estados Iberoamericanos, la OCDE, la OTAN y la Unión Europea —incluidos dentro de esta al espacio Schengen y la eurozona—, además de ser miembro de facto del G20.
La primera presencia constatada de homínidos del género Homo se remonta a 1,2 millones de años antes del presente, como atestigua el descubrimiento de una mandíbula de un Homo aún sin clasificar en el yacimiento de Atapuerca. En el siglo III a. C., se produjo la intervención romana en la Península, lo que conllevó a una posterior conquista de lo que, más tarde, se convertiría en Hispania. En el Medievo, la zona fue conquistada por distintos pueblos germánicos y por los musulmanes, llegando estos a tener presencia durante algo más de siete centurias. Es en el siglo XV, con la unión dinástica de las Coronas de Castilla y Aragón y la culminación de la Reconquista, junto con la posterior anexión navarra, cuando se puede hablar de la cimentación de «España», como era denominada en el exterior. Ya en la Edad Moderna, los monarcas españoles gobernaron el primer imperio de ultramar global, que abarcaba territorios en los cinco continentes, dejando un vasto acervo cultural y lingüístico por el globo. A principios del xix, tras sucesivas guerras en Hispanoamérica, pierde la mayoría de sus territorios en América, acrecentándose esta situación con el desastre del 98. Durante este siglo, se produciría también una guerra contra el invasor francés, una serie de guerras civiles, una efímera república reemplazada nuevamente por una monarquía constitucional y el proceso de modernización del país. En el primer tercio del siglo XX, se proclamó una república constitucional. Un golpe de Estado militar fallido provocó el estallido de una guerra civil, cuyo fin dio paso a la dictadura de Francisco Franco, finalizada con la muerte de este en 1975, momento en que se inició una transición hacia la democracia. Su clímax fue la redacción, ratificación en referéndum y promulgación de la Constitución de 1978.Acrecentado significativamente durante el llamado «milagro económico español», el desarrollo económico y social del país ha continuado a lo largo del vigente periodo democrático.
"),

("Spanish", "Patrimonio de la Humanidad", "https://es.wikipedia.org/wiki/Patrimonio_de_la_Humanidad", @"
«Patrimonio Mundial», más conocido como Patrimonio de la Humanidad, es el título conferido por Unesco a sitios específicos del planeta (sean bosque, montaña, lago, laguna, cueva, desierto, edificación, complejo arquitectónico, ruta cultural, paisaje cultural o ciudad) que han sido propuestos y confirmados para su inclusión en la lista mantenida por el programa Patrimonio Mundial, administrado por el Comité del Patrimonio Mundial, compuesto por 21 Estados miembros a los que elige la Asamblea General de Estados miembros por un periodo determinado.
Placa conmemorativa de la inscripción en la Lista del Patrimonio Mundial en Doñana, Huelva
El objetivo del programa es catalogar, preservar y dar a conocer sitios de importancia cultural o natural excepcional para la herencia común de la humanidad. Bajo ciertas condiciones, los sitios mencionados pueden obtener financiación para su conservación del Fondo para la conservación del Patrimonio mundial. Fue fundado por la Convención para la cooperación internacional en la protección de la herencia cultural y natural de la humanidad, que posteriormente fue adoptada por la conferencia general de la Unesco el 16 de noviembre de 1972. Desde entonces, 193 Estados miembros han ratificado la convención.
Para julio de 2024, el catálogo consta de un total de 1223 sitios del Patrimonio Mundial, de los cuales 952 son culturales, 231 naturales y 40 mixtos, distribuidos en 168 países.Por cantidad de sitios inscritos, los diez primeros países son: Italia (60 sitios), China (59 sitios), Alemania (54 sitios), Francia (53 sitios), España (50 sitios), India (43 sitios), México y Reino Unido (ambos con 35 sitios), Rusia (32 sitios) e Irán (28 sitios). Castilla y León y Andalucía (España) son algunas de las regiones del mundo con más bienes culturales Patrimonio Mundial, con 8; junto con las regiones de la Toscana, la Lombardía y el Veneto (Italia), las tres con 8 o más bienes.
Históricamente, la inscripción de los dos primeros bienes registrados en la Lista del Patrimonio Mundial en 1978, la Ciudad de Quito y las islas Galápagos, ambas ubicadas en Ecuador, se celebra cada 3 de diciembre en la Sede de la Unesco. Las islas Galápagos poseen la distinción de ser el primer bien natural declarado Patrimonio Mundial, mientras que la Ciudad de Quito, junto con el Centro histórico de Cracovia (Polonia), fueron los primeros centros históricos inscritos en la Lista.
La Ciudad de Quito fue inscrita en la Lista del Patrimonio Mundial bajo los criterios (ii) y (iv) por representar un importante intercambio de valores sociales entre los españoles y las poblaciones indígenas, lo cual resultó en el desarrollo de un conjunto arquitectónico y monumental único. Fundada en el siglo XVI sobre las ruinas de una ciudad inca, y a una altitud de 2850 m, la ciudad fue considerada por la comunidad del Patrimonio al momento de su inscripción como el centro histórico mejor conservado y menos alterado en América Latina.
Las islas Galápagos fueron inscritas en la Lista del Patrimonio Mundial bajo criterios (vii), (viii), (ix) y (x), mostrando un espectacular y único sitio natural caracterizado por un extraordinario valor en vida silvestre y expresión excepcional de los periodos de la historia del planeta que no se encuentra en ningún otro lugar del mundo. Asimismo, el bien contiene ejemplos excepcionales de procesos ecológicos y biológicos de la evolución de ecosistemas y hábitats para la conservación in situ de la diversidad biológica. Desde la publicación de El viaje del Beagle, por Charles Darwin en 1839, el origen de la flora y la fauna de Galápagos ha sido de gran interés académico e importancia para la conservación.
La Unesco se refiere a cada sitio con un número de identificación único, pero las nuevas inscripciones incluyen a menudo los sitios anteriores ahora enumerados como parte de descripciones más grandes. Consecuentemente, el sistema de numeración termina actualmente sobre 1500, aunque realmente haya 1092 catalogados; con el añadido de que muchos de los Patrimonios de la Humanidad se encuentran en múltiples ubicaciones, siendo el mismo sitio, principalmente aquellos que son rutas culturales, conjuntos de un mismo concepto de sitio natural protegido, o paisajes culturales.
Cada sitio Patrimonio Mundial pertenece al país en el que se localiza, pero se considera en el interés de la comunidad internacional y debe ser preservado para las futuras generaciones. La protección y la conservación de estos sitios son una preocupación de los 122 Estados miembros de la Unesco.
"),

("Spanish", "África del Norte", "https://es.wikipedia.org/wiki/%C3%81frica_del_Norte", @"
África del Norte, África septentrional o África norsahariana (a veces llamada África Blanca) es la subregión norte de África. Está compuesta por cinco países: Argelia, Egipto, Libia, Marruecos y Túnez. Además, incluye a la República Árabe Saharaui Democrática (que es un Estado con reconocimiento limitado) y otros territorios que dependen de países externos a la subregión: Canarias, Ceuta y Melilla (que dependen de España), Madeira (de Portugal) y Lampedusa e Linosa (de Italia).
Esta subregión limita al norte con el mar Mediterráneo, al este con el mar Rojo, al sur con África Oriental, África Central y África Occidental, y al oeste con el océano Atlántico.
El Magreb es parte de África noroccidental.
"),

("Spanish", "Peter Pan", null, @"
Había una vez un niño llamado Peter Pan. Peter vivía en un lugar mágico llamado Nunca Jamás. En este lugar, los niños nunca crecían. Peter Pan podía volar y siempre estaba acompañado por su amiga, un hada pequeña llamada Campanilla.
Una noche, Peter Pan voló hasta la casa de una niña llamada Wendy y sus hermanos, Juan y Miguel. Peter los invitó a viajar con él a Nunca Jamás. Wendy, Juan y Miguel aceptaron y, con la ayuda del polvo mágico de Campanilla, comenzaron a volar.
En Nunca Jamás, vivían los Niños Perdidos, que eran amigos de Peter. También había piratas malos, liderados por el Capitán Garfio, quien siempre quería atrapar a Peter Pan. El capitán Garfio tenía un gancho en lugar de una mano, porque Peter se la cortó en una pelea y se la dio a un cocodrilo.
Los niños vivieron muchas aventuras en Nunca Jamás, pero Wendy y sus hermanos comenzaron a extrañar su hogar. Decidieron regresar con Peter Pan, pero el Capitán Garfio los atrapó.
Con la ayuda de Campanilla, Peter Pan rescató a Wendy, Juan y Miguel. Finalmente, los niños regresaron a casa, pero Peter Pan decidió quedarse en Nunca Jamás, donde podía seguir siendo un niño para siempre.
"),


("Spanish", "Papá Noel", null, @"
Papá Noel, conocido también como Santa Claus (en neerlandés: Sinterklaas), Viejito Pascuero, Colacho, San Nicolás o simplemente Santa, es un personaje legendario originario del cristianismo occidental, conocido por repartir regalos a niños durante las noches de Nochebuena y Navidad (24 y 25 de diciembre). Estos regalos pueden contener juguetes, golosinas, carbón o simplemente nada, dependiendo de si el infante se encuentra en la «lista de niños buenos o malos». Según la leyenda, Papá Noel fabrica los regalos con ayuda de sus elfos, con los que trabaja en su taller, repartiendo los regalos con ayuda de sus renos que tiran de su trineo por el aire.
El personaje de Papá Noel está inspirado en tradiciones folclóricas destacadas de personajes como Nicolás de Bari (como también de su versión legendaria húngara, Mikulás) por parte del folclore europeo, la personificación de la Navidad según el folclore inglés de Papá Noel, y la figura neerlandesa de Sinterklaas."
),

("English", "The Selfish Giant", "https://www.wilde-online.info/the-selfish-giant.html", @"
The Selfish Giant
by Oscar Wilde
Every afternoon, as they were coming from school, the children used to go and play in the Giant’s garden.
It was a large lovely garden, with soft green grass. Here and there over the grass stood beautiful flowers like stars, and there were twelve peach-trees that in the spring-time broke out into delicate blossoms of pink and pearl, and in the autumn bore rich fruit. The birds sat on the trees and sang so sweetly that the children used to stop their games in order to listen to them. “How happy we are here!” they cried to each other.
One day the Giant came back. He had been to visit his friend the Cornish ogre, and had stayed with him for seven years. After the seven years were over he had said all that he had to say, for his conversation was limited, and he determined to return to his own castle. When he arrived he saw the children playing in the garden.
“What are you doing here?” he cried in a very gruff voice, and the children ran away.
“My own garden is my own garden,” said the Giant; “any one can understand that, and I will allow nobody to play in it but myself.” So he built a high wall all round it, and put up a notice-board.
TRESPASSERS WILL BE PROSECUTED
He was a very selfish Giant.
The poor children had now nowhere to play. They tried to play on the road, but the road was very dusty and full of hard stones, and they did not like it. They used to wander round the high wall when their lessons were over, and talk about the beautiful garden inside. “How happy we were there,” they said to each other.
Then the Spring came, and all over the country there were little blossoms and little birds. Only in the garden of the Selfish Giant it was still winter. The birds did not care to sing in it as there were no children, and the trees forgot to blossom. Once a beautiful flower put its head out from the grass, but when it saw the notice-board it was so sorry for the children that it slipped back into the ground again, and went off to sleep. The only people who were pleased were the Snow and the Frost. “Spring has forgotten this garden,” they cried, “so we will live here all the year round.” The Snow covered up the grass with her great white cloak, and the Frost painted all the trees silver. Then they invited the North Wind to stay with them, and he came. He was wrapped in furs, and he roared all day about the garden, and blew the chimney-pots down. “This is a delightful spot,” he said, “we must ask the Hail on a visit.” So the Hail came. Every day for three hours he rattled on the roof of the castle till he broke most of the slates, and then he ran round and round the garden as fast as he could go. He was dressed in grey, and his breath was like ice.
“I cannot understand why the Spring is so late in coming,” said the Selfish Giant, as he sat at the window and looked out at his cold white garden; “I hope there will be a change in the weather.”
But the Spring never came, nor the Summer. The Autumn gave golden fruit to every garden, but to the Giant’s garden she gave none. “He is too selfish,” she said. So it was always Winter there, and the North Wind, and the Hail, and the Frost, and the Snow danced about through the trees.
One morning the Giant was lying awake in bed when he heard some lovely music. It sounded so sweet to his ears that he thought it must be the King’s musicians passing by.
It was really only a little linnet singing outside his window, but it was so long since he had heard a bird sing in his garden that it seemed to him to be the most beautiful music in the world. Then the Hail stopped dancing over his head, and the North Wind ceased roaring, and a delicious perfume came to him through the open casement. “I believe the Spring has come at last,” said the Giant; and he jumped out of bed and looked out.
What did he see?
He saw a most wonderful sight. Through a little hole in the wall the children had crept in, and they were sitting in the branches of the trees. In every tree that he could see there was a little child. And the trees were so glad to have the children back again that they had covered themselves with blossoms, and were waving their arms gently above the children’s heads. The birds were flying about and twittering with delight, and the flowers were looking up through the green grass and laughing. It was a lovely scene, only in one corner it was still winter. It was the farthest corner of the garden, and in it was standing a little boy. He was so small that he could not reach up to the branches of the tree, and he was wandering all round it, crying bitterly. The poor tree was still quite covered with frost and snow, and the North Wind was blowing and roaring above it. “Climb up! little boy,” said the Tree, and it bent its branches down as low as it could; but the boy was too tiny.
And the Giant’s heart melted as he looked out. “How selfish I have been!” he said; “now I know why the Spring would not come here. I will put that poor little boy on the top of the tree, and then I will knock down the wall, and my garden shall be the children’s playground for ever and ever.” He was really very sorry for what he had done.
So he crept downstairs and opened the front door quite softly, and went out into the garden. But when the children saw him they were so frightened that they all ran away, and the garden became winter again. Only the little boy did not run, for his eyes were so full of tears that he did not see the Giant coming. And the Giant stole up behind him and took him gently in his hand, and put him up into the tree. And the tree broke at once into blossom, and the birds came and sang on it, and the little boy stretched out his two arms and flung them round the Giant’s neck, and kissed him. And the other children, when they saw that the Giant was not wicked any longer, came running back, and with them came the Spring. “It is your garden now, little children,” said the Giant, and he took a great axe and knocked down the wall. And when the people were going to market at twelve o’clock they found the Giant playing with the children in the most beautiful garden they had ever seen.
All day long they played, and in the evening they came to the Giant to bid him good-bye.
“But where is your little companion?” he said: “the boy I put into the tree.” The Giant loved him the best because he had kissed him.
“We don’t know,” answered the children; “he has gone away.”
“You must tell him to be sure and come here to-morrow,” said the Giant. But the children said that they did not know where he lived, and had never seen him before; and the Giant felt very sad.
Every afternoon, when school was over, the children came and played with the Giant. But the little boy whom the Giant loved was never seen again.
The Giant was very kind to all the children, yet he longed for his first little friend, and often spoke of him. “How I would like to see him!” he used to say.
Years went over, and the Giant grew very old and feeble. He could not play about any more, so he sat in a huge armchair, and watched the children at their games, and admired his garden. “I have many beautiful flowers,” he said; “but the children are the most beautiful flowers of all.”
One winter morning he looked out of his window as he was dressing. He did not hate the Winter now, for he knew that it was merely the Spring asleep, and that the flowers were resting.
Suddenly he rubbed his eyes in wonder, and looked and looked. It certainly was a marvellous sight. In the farthest corner of the garden was a tree quite covered with lovely white blossoms. Its branches were all golden, and silver fruit hung down from them, and underneath it stood the little boy he had loved.
Downstairs ran the Giant in great joy, and out into the garden. He hastened across the grass, and came near to the child. And when he came quite close his face grew red with anger, and he said, “Who hath dared to wound thee?” For on the palms of the child’s hands were the prints of two nails, and the prints of two nails were on the little feet.
“Who hath dared to wound thee?” cried the Giant; “tell me, that I may take my big sword and slay him.”
“Nay!” answered the child; “but these are the wounds of Love.”
“Who art thou?” said the Giant, and a strange awe fell on him, and he knelt before the little child.
And the child smiled on the Giant, and said to him, “You let me play once in your garden, to-day you shall come with me to my garden, which is Paradise.”
And when the children ran in that afternoon, they found the Giant lying dead under the tree, all covered with white blossoms.
"),

];
    }
}

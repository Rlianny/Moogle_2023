# !/bin/bash

# Función para mostrar el menú
mostrar_menu() {
    echo "selecciona una opción:"
    echo "1. run"
    echo "2. report"
    echo "3. slides"
    echo "4. show_repot"
    echo "5. show_slides"
    echo "6. clean"
    echo "7. Salir"
}

# Función para leer la opción seleccionada
leer_opcion() {
    echo "Ingresa tu opción:"
    read opcion
}

# Bucle principal del menú
opcion=0
while [ $opcion -ne 7 ]; do
    mostrar_menu
    leer_opcion
    clear  # Limpia la pantalla

    case $opcion in
        1)
            echo "Has seleccionado la opción 1  "
            cd ..
            dotnet watch run --project MoogleServer
            cd script
            ;;
        2)
            echo "Has seleccionado la opción 2 "
            cd ..
            cd Informe
            pdflatex informe.tex 
            cd ..
            cd script


            ;;
        3)
            echo "Has seleccionado la opción 3 "
            cd ..
            cd Presentación 
            pdflatex presentación.tex 
            cd ..
            cd script

            ;;
        4)
            echo "Has seleccionado la opción 4"
            echo "Visualizando el informe..."
            cd ..
            cd Informe
            if [ ! -f informe.pdf ]; then
            echo "Compilando y generando el PDF del informe..."
            pdflatex informe.tex
            fi
            xdg-open informe.pdf 
            cd ..
            cd script

            ;;
        5)
            echo "Has seleccionado la opción 5"
            echo "Visualizando la presentacion..."
            cd ..
            cd Presentación
            if [ ! -f presentación.pdf ]; then
            echo "Compilando y generando el PDF de la Presentación..."
            pdflatex presentación.tex
            fi
            xdg-open presentación.pdf 
            cd ..
            cd script

            ;;
        6)
            echo "Has seleccionado la opción 6"
            cd ..
            cd Informe
            rm -f *.log *.gz *.aux *.nav *.snm *.toc *.pdf
           

            cd ..
            cd Presentación
            rm -f *.log *.gz *.aux *.nav *.snm *.toc *.pdf
            
            
            cd ..
            cd MoogleEngine
            rm -r bin
            rm -r obj
            
            cd ..
            cd MoogleServer
            rm -r bin
            rm -r obj
            
            echo "Todo limpio, gracias "
            ;;
        7)
            echo "Saliendo ..."
            ;;
        *)
            echo "Opción inválida, por favor selecciona una opción válida"
            ;;
    esac

    # Espera a que el usuario presione Enter antes de continuar
    echo "Presiona Enter para continuar..."
    read -r
    clear  # Limpia la pantalla
done
# PlayStationStarsPointsExtractor

Application console qui permet de convertir l'extrait du code html de l'hitorique de points PlayStation Star en fichier csv.

## Marche à suivre

- Aller sur playstation.com
- Après vous être connecté cliquez sur votre avatar
- Dans le menu déroulant cliquez sur la section avec les points
- Sur la page trouvez et cliquez sur le lien "Voir votre activié".
- Un panneau va apparaître sur la droite.  Faite un clique doit sur le panneau et ouvrer l'inspecteur(la plus part des navigateurs à cette options en faisant un cliique droit.  Même safari sur macOS).
- Copiez le noeud `<ul class="transactions-list">`
- Coller dans un fichier nommé "transation.html"
- Laissez la mogie opérer et un fichier csv sera généré.

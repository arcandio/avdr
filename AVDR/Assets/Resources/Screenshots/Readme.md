The **Screenshots** folder is for content generated for the build in the unity editor by using `Menu/AVDR Screenshots/Capture All Assets`.

This produces thumbnails for the content in the app.

The generation process results in images with slightly randomized shading in the shadows, causing every generated image to be added to version control regardless of how similiar they are to the existing images.

Because the generated assets can simply be regenerated from a copy of the repo and they would unneccisarily fill up the repo storage, we omit them from the repo.

An additional benefit of this is that we can generate very big screenshots to use for marketing and not worry about storing them in the repo, since we can simply use an asset import preset to resize them smaller for the build.

See the `.gitignore` file for git rules.
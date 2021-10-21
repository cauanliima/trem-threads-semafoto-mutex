ULTIMA_TAG="$(git describe --tags)"
TIMESTAMP="$(date '+%Y%m%d%H%M%S')"

if [[ -z "$ULTIMA_TAG" ]]; then
        ULTIMA_TAG="1.0.0"
fi

NOVA_VERSAO="$ULTIMA_TAG-rev+$TIMESTAMP.sha1.$CI_COMMIT_SHORT_SHA"

echo $NOVA_VERSAO
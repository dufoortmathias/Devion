<template>
    <div class="c-artikel_form" :disabled="disabled">
        <textInput :id="artikelNr.id" :label="artikelNr.label" :error="artikelNr.error" :placeholder="artikelNr.label"
            class="c-artikelNr" />
        <textInput :id="reflev.id" :label="reflev.label" :error="reflev.error" :placeholder="reflev.label"
            class="c-reflev" />
        <textInput :id="omschrijving.id" :label="omschrijving.label" :error="omschrijving.error"
            :placeholder="omschrijving.label" class="c-omschrijving" />
        <textInput :id="tarif.id" :label="tarif.label" :error="tarif.error" :placeholder="tarif.label" class="c-tarif" />
        <textInput :id="aankoop.id" :label="aankoop.label" :error="aankoop.error" :placeholder="aankoop.label"
            class="c-aankoop" />
        <textInput :id="stdKorting.id" :label="stdKorting.label" :error="stdKorting.error"
            :placeholder="stdKorting.label" class="c-stdKorting" />
        <dropdownMenu :id="muntcode.id" :label="muntcode.label" :options="muntcode.options" :error="muntcode.error"
            class="c-muntcode" />
        <textInput :id="verkoop.id" :label="verkoop.label" :error="verkoop.error" :placeholder="verkoop.label"
            class="c-verkoop" />
        <textInput :id="winstpercentage.id" :label="winstpercentage.label" :error="winstpercentage.error" :placeholder="winstpercentage.label"
            class="c-winstpercentage" />
        <dropdownMenu :id="rekver.id" :label="rekver.label" :options="rekver.options" :error="rekver.error" class="c-rekver" />
        <dropdownMenu :id="aaneh.id" :label="aaneh.label" :options="aaneh.options" :error="aaneh.error" class="c-aaneh" />
        <dropdownMenu :id="vereh.id" :label="vereh.label" :options="vereh.options" :error="vereh.error" class="c-vereh" />
        <dropdownMenu :id="btwcode.id" :label="btwcode.label" :options="btwcode.options" :error="btwcode.error" class="c-btwcode" />
        <textInput :id="omrekfac.id" :label="omrekfac.label" :error="omrekfac.error" :placeholder="omrekfac.label"
            class="c-omrekfac" />
        <dropdownMenu :id="typfac.id" :label="typfac.label" :options="typfac.options" :error="typfac.error" class="c-typfac" />
        <textInput :id="merk.id" :label="merk.label" :error="merk.error" :placeholder="merk.label" class="c-merk" />
        <dropdownMenu :id="familie.id" :label="familie.label" :options="familie.options" :error="familie.error" class="c-familie" />
        <dropdownMenu :id="subfamilie.id" :label="subfamilie.label" :options="subfamilie.options" :error="subfamilie.error" class="c-subfamilie" />
        <textInput :id="lengte.id" :label="lengte.label" :error="lengte.error" :placeholder="lengte.label" class="c-lengte" />
        <textInput :id="breedte.id" :label="breedte.label" :error="breedte.error" :placeholder="breedte.label" class="c-breedte" />
        <textInput :id="hoogte.id" :label="hoogte.label" :error="hoogte.error" :placeholder="hoogte.label" class="c-hoogte" />
        <dropdownMenu :id="hoofdleverancier.id" :label="hoofdleverancier.label" :options="hoofdleverancier.options" :error="hoofdleverancier.error" class="c-hoofdleverancier" />
        <textInput :id="minaan.id" :label="minaan.label" :error="minaan.error" :placeholder="minaan.label" class="c-minaan" />
    </div>
</template>

<script>
import textInput from '../components/componenten/textInput.vue';
import dropdownMenu from '../components/componenten/DropdownMenu.vue';

export default {
    props: {
        disabled: Boolean,
    },
    components: {
        textInput,
        dropdownMenu
    },
    data() {
        return {
            artikelNr: {
                components: {
                    textInput,
                },
                id: 'artikelNr',
                label: 'Artikel nummer',
                error: false,
            },
            reflev: {
                components: {
                    textInput,
                },
                id: 'reflev',
                label: 'Referentie leverancier',
                error: false,
            },
            omschrijving: {
                components: {
                    textInput,
                },
                id: 'omschrijving',
                label: 'Omschrijving',
                error: false,
            },
            tarif: {
                components: {
                    textInput,
                },
                id: 'tarif',
                label: 'Catalogus prijs',
                error: false,
            },
            aankoop: {
                components: {
                    textInput,
                },
                id: 'aankoop',
                label: 'Aankoop prijs',
                error: false,
            },
            stdKorting: {
                components: {
                    textInput,
                },
                id: 'stdKorting',
                label: 'Standaard korting (%)',
                error: false,
            },
            muntcode: {
                components: {
                    dropdownMenu,
                },
                id: 'muntcode',
                label: 'Muntcode',
                options: [],
                error: false,
            },
            verkoop: {
                components: {
                    textInput,
                },
                id: 'verkoop',
                label: 'Verkoop prijs',
                error: false,
            },
            winstpercentage: {
                components: {
                    textInput,
                },
                id: 'winstpercentage',
                label: 'Winst percentage (%)',
                error: false,
            },
            rekver: {
                components: {
                    dropdownMenu,
                },
                id: 'rekver',
                label: 'Rekening verkoop',
                options: [],
                error: false,
            },
            aaneh: {
                components: {
                    dropdownMenu,
                },
                id: 'aaneh',
                label: 'Aankoop eenheid',
                options: [],
                error: false,
            },
            vereh: {
                components: {
                    dropdownMenu,
                },
                id: 'vereh',
                label: 'Verkoop eenheid',
                options: [],
                error: false,
            },
            btwcode: {
                components: {
                    dropdownMenu,
                },
                id: 'btwcode',
                label: 'BTW code',
                options: [],
                error: false,
            },
            omrekfac: {
                components: {
                    textInput,
                },
                id: 'omrekfac',
                label: 'Omrekeningsfactor',
                error: false,
            },
            typfac: {
                components: {
                    dropdownMenu,
                },
                id: 'typfac',
                label: 'Type factor',
                options: [],
                error: false,
            },
            merk: {
                components: {
                    textInput,
                },
                id: 'merk',
                label: 'Merk',
                error: false,
            },
            familie: {
                components: {
                    dropdownMenu,
                },
                id: 'familie',
                label: 'Familie',
                options: [],
                error: false,
            },
            subfamilie: {
                components: {
                    dropdownMenu,
                },
                id: 'subfamilie',
                label: 'Subfamilie',
                options: [],
                error: false,
            },
            lengte: {
                components: {
                    textInput,
                },
                id: 'lengte',
                label: 'Lengte',
                error: false,
            },
            breedte: {
                components: {
                    textInput,
                },
                id: 'breedte',
                label: 'Breedte',
                error: false,
            },
            hoogte: {
                components: {
                    textInput,
                },
                id: 'hoogte',
                label: 'Hoogte',
                error: false,
            },
            hoofdleverancier: {
                components: {
                    dropdownMenu,
                },
                id: 'hoofdleverancier',
                label: 'Hoofdleverancier',
                options: [],
                error: false,
            },
            minaan: {
                components: {
                    textInput,
                },
                id: 'minaan',
                label: 'Minimum aantal',
                error: false,
            },
        }
    }
}
</script>

<style scoped>
.c-artikel_form {
    display: grid;
    grid-template-areas:
        "artikelnr reflevnr omschrijving"
        "tarief prijs stdkorting"
        "muntcode verkoop winst"
        "rekver akprijseh verbruikseh"
        "btwcode omrekfac typfac"
        "merk familie subfamilie"
        "lengte breedte hoogte"
        "hoofdleverancier link minaan"
        "prev progress next";
    grid-template-columns: 30vw 30vw 30vw;
    grid-gap: var(--global-whitespace-lg);
    width: 100vw;
    transform: translateX(-15vw);
}

.c-artikelNr {
    grid-area: artikelnr;
}

.c-reflev {
    grid-area: reflevnr;
}

.c-omschrijving {
    grid-area: omschrijving;
}

.c-tarif {
    grid-area: tarief;
}

.c-aankoop {
    grid-area: prijs;
}

.c-stdKorting {
    grid-area: stdkorting;
}

.c-muntcode {
    grid-area: muntcode;
}

.c-verkoop {
    grid-area: verkoop;
}

.c-winstpercentage {
    grid-area: winst;
}

.c-rekver {
    grid-area: rekver;
}

.c-aaneh {
    grid-area: akprijseh;
}

.c-vereh {
    grid-area: verbruikseh;
}

.c-btwcode {
    grid-area: btwcode;
}

.c-omrekfac {
    grid-area: omrekfac;
}

.c-omrekfac {
    grid-area: omrekfac;
}

.c-merk {
    grid-area: merk;
}

.c-familie {
    grid-area: familie;
}

.c-subfamilie {
    grid-area: subfamilie;
}

.c-lengte {
    grid-area: lengte;
}

.c-breedte {
    grid-area: breedte;
}

.c-hoogte {
    grid-area: hoogte;
}

.c-hoofdleverancier {
    grid-area: hoofdleverancier;
}

.c-minaan {
    grid-area: minaan;
}
</style>
<template>
    <div class="c-artikel-link">
        <label for="" class="c-label c-label-link">link</label>
        <a href="#" class="c-link c-link-artikel js-link-icon" target="_blank">?</a>
        <input type="url" class="c-input c-input-link js-input-link" placeholder="link" name="link" id="link" disabled />
    </div>
</template>

<style scoped>
.c-label-link {
    padding-top: 11px;
    grid-area: label;
}

.c-input-link {
    grid-area: input;
}

.c-link-artikel {
    grid-area: icon;
    color: #fff;
    justify-self: end;
    align-self: center;
    background-color: var(--global-color-alpha);
    width: var(--global-whitespace-md);
    height: var(--global-whitespace-md);
    display: inline-block;
    border-radius: 100%;
    font-size: 10px;
    text-align: center;
    text-decoration: none;
    -webkit-box-shadow: inset -1px -1px 1px 0px rgba(0, 0, 0, 0.25);
    -moz-box-shadow: inset -1px -1px 1px 0px rgba(0, 0, 0, 0.25);
    box-shadow: inset -1px -1px 1px 0px rgba(0, 0, 0, 0.25);
    margin-right: var(--global-whitespace-md);
}

.c-link-artikel:hover,
.c-link-artikel:visited,
.c-link-artikel:active {
    color: var(--global-color-neutral-xxxx-light)
}
</style>
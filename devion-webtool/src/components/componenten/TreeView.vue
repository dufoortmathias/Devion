<template>
    <ul class="treeview" v-show="showTree">
      <li v-for="(part, index) in jsonData" :key="index" class="c-placement">
        <div>
          <span
            @click="toggleNode(part)"
            v-if="part && part != null"
            :class="{
              caret: hasChildParts(part),
              'no-caret': !hasChildParts(part),
              'caret-down': part.expanded,
              exists: part.existsDev,
              'not-exists': !part.existsDev,
              changed: part.changedDev
            }"
            class="c-text"
          >
            {{ part.number }} ({{ part.description }})
          </span>
        </div>
        <ul v-if="part && part.parts && part.expanded && part.parts">
          <TreeView :jsonData="part.parts" :showTree="true" />
        </ul>
      </li>
    </ul>
  </template>

<script>
export default {
  name: 'TreeView',
  props: {
    jsonData: Array,
    showTree: Boolean
  },
  components: {},
  data() {
    return {
      expanded: false
    }
  },
  methods: {
    toggleNode(node) {
      node.expanded = !node.expanded
    },
    hasChildParts(part) {
      return part.parts && part.parts.length > 0
    }
  }
}
</script>

<style scoped>
.treeview {
  margin-left: 20px;
  list-style-type: none;
}

.node {
  display: flex;
  align-items: center;
  margin: 5px 0;
}

.node-label {
  flex-grow: 1;
}

.node-quantity {
  margin-right: 10px;
}

.caret {
  cursor: pointer;
  user-select: none;
}

.no-caret {
  cursor: default;
  user-select: none;
  margin-left: 20px;
}

.caret::before {
  content: '\25B6';
  color: black;
  display: inline-block;
  margin-right: 6px;
}

.caret-down::before {
  transform: rotate(90deg);
}

.exists {
  color: green;
}

.not-exists {
  color: red;
}

.changed {
  color: darkorange;
}

.c-placement {
  margin-top: 3vh;
}

.c-text {
  font-size: var(--global-font-size);
  font-weight: var(--global-font-weight-bold);
  padding-top: 11px;
  grid-area: label;
}

.c-button {
  margin: 0;
  cursor: pointer;
  grid-area: button;
  float: right;
  width: 20%;
}
</style>
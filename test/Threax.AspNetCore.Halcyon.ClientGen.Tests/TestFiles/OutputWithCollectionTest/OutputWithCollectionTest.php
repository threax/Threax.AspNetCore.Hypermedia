<?php

namespace phptest\client;
use threax\halcyonclient\HalEndpointClient;
use threax\halcyonclient\CurlHelper;

class OutputCollectionResult {
    private $client;

    public function __construct(HalEndpointClient $client) {
        $this->client = $client;
    }

    public function getData() {
        return $this->client->getData();
    }

    private $itemsStrong = NULL;
    public function getItems(): array {
        if ($this->itemsStrong === NULL) {
            $embeds = $this->client->getEmbed("values");
            $clients = $embeds->getAllClients();
            $this->itemsStrong = [];
            foreach ($clients as $client) {
                array_push($this->itemsStrong, new OutputResult($client));
            }
        }
        return $this->itemsStrong;
    }

    public function save($data) {
        $r = $this->client->loadLinkWithData("Save", $data);
    }

    public function canSave(): bool {
        return $this->client->hasLink("Save");
    }

    public function linkForSave() {
        return $this->client->getLink("Save");
    }

    public function getSaveDocs(HalEndpointDocQuery $query = NULL) {
        return $this->client->loadLinkDoc("Save", $query)->getData();
    }

    public function hasSaveDocs(): bool {
        return $this->client->hasLinkDoc("Save");
    }
}

class HalEndpointDocQuery {
    public $includeRequest;
    public $includeResponse;
}

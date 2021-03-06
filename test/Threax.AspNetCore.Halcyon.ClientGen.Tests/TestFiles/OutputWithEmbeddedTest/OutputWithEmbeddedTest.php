<?php

namespace phptest\client;
use threax\halcyonclient\HalEndpointClient;
use threax\halcyonclient\CurlHelper;

class OutputResult {
    private $client;

    public function __construct(HalEndpointClient $client) {
        $this->client = $client;
    }

    public function getData() {
        return $this->client->getData();
    }

    private $embedsStrong = NULL;
    public function getEmbeds(): array {
        if ($this->embedsStrong === NULL) {
            $embeds = $this->client->getEmbed("embeds");
            $clients = $embeds->getAllClients();
            $this->embedsStrong = [];
            foreach ($clients as $client) {
                array_push($this->embedsStrong, new EmbeddedObjectResult($client));
            }
        }
        return $this->embedsStrong;
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
